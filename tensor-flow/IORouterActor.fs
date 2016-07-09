module IORouterActor

//Create single writer actor and bunch of readers
//While read could be done in parallel mapped outputs will be sync via single writer.

// IORouter - coordinator, spawn one writer and pool of redaer routers

open System
open Akka.Actor
open Akka.FSharp
open types


open WriterActor
open ReaderActor
open DataProccessing.Types


let IORouterActor (mailbox: Actor<IORouterMessages>) = 
    
    let rec router() = 
        actor {
            
            let! msg = mailbox.Receive()

            match msg with 
            | IORouterStart paths ->

                let writer = spawn mailbox "Writer" (WriterActor mailbox.Self)
                writer <! WriterStart paths.output                
                                                
                let path, filter = 
                    match paths.input with
                    | DirPath path -> path, "*"
                    | DirPathFilter(path, filter) -> (path, "*." + filter)

                let routerOpt = SpawnOption.Router ( Akka.Routing.FromConfig.Instance )
                let supervisionOpt = SpawnOption.SupervisorStrategy (Strategy.OneForOne(fun _ ->
                     Directive.Resume
                ))
                                
                let reader = spawnOpt mailbox "Reader" (FileReaderActor mailbox.Self writer) [routerOpt; supervisionOpt]

                let files = System.IO.Directory.GetFiles(path, filter, IO.SearchOption.AllDirectories);
                                
                files|> Array.iter (fun path -> reader <! (ReaderFileRead path))   
                
                return! waitComplete writer files.Length 
            | _ ->             
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
                logDebugf mailbox "File write complete (left %i)" leftCount
                if leftCount = 0 then
                    logInfo mailbox "Stop writer"
                    async { return! writer <? WriterStop } |!> mailbox.Self
                return! waitComplete writer leftCount
            | IORouterWriterClosed ->                
                mailbox.Context.System.Terminate() |> ignore                                            
            | _ ->
                return! waitComplete writer cnt
    }

        
    router()
    
