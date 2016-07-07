module ReaderActor


open System
open Akka.Actor
open Akka.FSharp
open types
open ImageFileReader
open maybe
open WriterActor

let INPUT_FILES_BATCH_SIZE = 5000
// TODO 
// separate WaitComplete actor
// supervision strategy - stop for file reader, continue for batch reader
// use pickler to pass messages data to writer

type ReaderMessage =
    // labels + files to read
    | ReaderStart of DirPath
    | ReaderFileRead of string   
    | StartReadFiles of int

let getLetterNumber (letter: char) =      
    byte <| int letter - 97

let mapPath2Label (path: string) = 
  (new System.IO.FileInfo(path)).Directory.Name.ToLower().[0]
  |> getLetterNumber

let FileReaderActor (ioRouter: IActorRef) (writer: IActorRef) (mailbox: Actor<ReaderMessage>) = 
               
    let rec reader() = 
        actor {
    
            let! msg = mailbox.Receive()
            let parent = mailbox.Context.Sender

            match msg with 
            | ReaderFileRead path -> 
                
                match readImage {width = 28; height = 28} path with 
                | Some image ->
                    let bytes = flat2dArray image
                    writer <! WriterWrite (mapPath2Label path, bytes)
                | None ->
                    ioRouter <! IORouterWriteComplete                                        
                return! reader()
            | _ -> 
                return! reader()

            return! reader()                
        }

    reader()

