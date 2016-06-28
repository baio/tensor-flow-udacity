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

type IORouterMessages = 
    | IORouterStart of InputOutputPaths

let IORouterActor (mailbox: Actor<IORouterMessages>) = 
    
    let rec router() = 
        actor {
            
            let! msg = mailbox.Receive()

            match msg with 
            | IORouterStart paths ->
                let writer = spawn mailbox "writerActor" WriterActor
                let reader = spawn mailbox "readerActor" (ReaderActor writer)

                writer <! WriterStart paths.output
                reader <! ReaderStart paths.input
        }
        
    router()
    
