module WriterActor


open System
open Akka.Actor
open Akka.FSharp
open types
open reader

type WriterMessage =
    // path of file
    | WriterStart of string
    // line to write
    | WriterWrite of string
    // stop to write
    | WriterStop

let WriterActor (mailbox: Actor<WriterMessage>) = 

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
            | WriterWrite line ->
                if streamWriter = null then
                    raise(Exception("Output file is not initialized"))
                else
                    streamWriter.Write line
                    streamWriter.Write "\n"
                return! writer()
            | WriterStop ->
                close()
        }

    writer()
    
