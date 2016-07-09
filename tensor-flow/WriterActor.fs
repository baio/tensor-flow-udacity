module WriterActor


open System
open Akka.Actor
open Akka.FSharp
open types

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
                    streamWriter.Write label
                    inputs |> Array.iter streamWriter.Write
                    streamWriter.Write "\n"
                    ioRouter <! IORouterWriteComplete
                return! writer()
            | WriterStop ->
                close()
                ioRouter <! IORouterWriterClosed
        }

    writer()
    
