module WriterActor


open System
open Akka.Actor
open Akka.FSharp
open types
open reader

open Nessos.FsPickler

type WriterMessage =
    // path of file
    | WriterStart of string
    // label + line to write
    | WriterWrite of byte * byte array
    // stop to write
    | WriterStop

let WriterActor (ioRouter: IActorRef) (mailbox: Actor<WriterMessage>) = 

    let binarySerializer = FsPickler.CreateBinary()
    let mutable streamWriter : System.IO.StreamWriter = null;

    let close () =  
        if streamWriter <> null then
            streamWriter.Close()
            streamWriter.Dispose()
            streamWriter <- null                

    mailbox.Defer close
        
    let rec writer() = 
        actor {
    
            let! msg = mailbox.Receive()

            match msg with 
            | WriterStart path -> 
                if streamWriter <> null then
                    raise(Exception("Output file access error"))
                else
                    streamWriter <- new System.IO.StreamWriter(new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                return! writer()
            | WriterWrite (label, inputs) ->
                if streamWriter = null then
                    raise(Exception("Output file is not initialized"))
                else
                    //binarySerializer.Serialize(streamWriter, inputs, leaveOpen = true)
                    let line = sprintf "%i%s" label (String.concat "" (inputs |> Array.map string))
                    streamWriter.Write line
                    streamWriter.Write "\n"
                    ioRouter <! IORouterWriteComplete
                return! writer()
            | WriterStop ->
                close()
                mailbox.Context.System.Terminate() |> ignore                                            
        }

    writer()
    
