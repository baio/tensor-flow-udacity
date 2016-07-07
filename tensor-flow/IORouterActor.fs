module IORouterActor

//Create single writer actor and bunch of readers
//While read could be done in parallel mapped outputs will be sync via single writer.

// IORouter - create writer & readers
// Reader, read files in parallel batches 
// Writer write all messages in single output file.

open System
open Akka.Actor
open Akka.FSharp
open types
open reader

open WriterActor
open ReaderActor


let IORouterActor (mailbox: Actor<IORouterMessages>) = 
    
    let rec router() = 
        actor {
            
            let! msg = mailbox.Receive()

            match msg with 
            | IORouterStart paths ->

                let writer = spawn mailbox "writerActor" (WriterActor mailbox.Self)
                writer <! WriterStart paths.output                
                                                
                let path, filter = 
                    match paths.input with
                    | DirPath path -> path, "*"
                    | DirPathFilter(path, filter) -> (path, "*." + filter)

                let routerOpt = SpawnOption.Router ( Akka.Routing.RoundRobinPool(10) )
                let supervisionOpt = SpawnOption.SupervisorStrategy (Strategy.OneForOne(fun _ ->
                     Directive.Resume
                ))
                                
                let reader = spawnOpt mailbox "batchReaderActor" (FileReaderActor mailbox.Self writer) [routerOpt; supervisionOpt]

                let files = System.IO.Directory.GetFiles(path, filter, IO.SearchOption.AllDirectories);
                                
                files|> Array.iter (fun path -> reader <! (ReaderFileRead path))   
                
                return! waitComplete writer files.Length 
            | IORouterWriteComplete -> 
                mailbox.Stash()
                return! router()                                         
               
        }
    and waitComplete (writer: IActorRef) (cnt: int) = 
        mailbox.UnstashAll()
        actor {                        
            let! message = mailbox.Receive()
            match message with
            | IORouterWriteComplete ->                
                let leftCount = cnt - 1
                printfn "Stop file read  (left %i)" leftCount
                if leftCount > 0 then
                    return! waitComplete writer leftCount
                else 
                    printfn "stop"
                    //Stop writer and stop actor
                    writer <! WriterStop
            | _ ->
                return! waitComplete writer cnt
    }

        
    router()
    
