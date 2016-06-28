module ReaderActor


open System
open Akka.Actor
open Akka.FSharp
open types
open ImageFileReader
open maybe
open WriterActor

let INPUT_FILES_BATCH_SIZE = 30

type ReaderMessage =
    // labels + files to read
    | ReaderStart of DirPath
    | ReaderBatchRead of string array
    | ReaderFileRead of string   
    | ReaderFileReadComplete
    | ReaderBatchReadComplete
    // stop read
    | ReaderStop

let mapPath2Label (path) = path

let FileReaderActor (writer: IActorRef) (mailbox: Actor<ReaderMessage>) = 
               
    let rec reader() = 
        actor {
    
            let! msg = mailbox.Receive()

            match msg with 
            | ReaderFileRead path -> 
                maybe {
                    let! image = readImage {width = 28; height = 28} path                    
                    let bytes = flat2dArray image
                                 |> Array.map string
                                 |> System.String.Concat                    
                    writer <! WriterWrite ((mapPath2Label path) + bytes)
                    mailbox.Sender() <! ReaderFileReadComplete
                } |> ignore
            | _ -> 
                return! reader()
                
        }

    reader()

let BatchReaderActor (writer: IActorRef) (mailbox: Actor<ReaderMessage>) = 
    mailbox.Defer (fun () -> mailbox.Sender() <! ReaderBatchReadComplete)
    let rec reader() = 
        actor {
            
            let! message = mailbox.Receive()

            match message with
            | ReaderBatchRead paths ->                
                let fileReader = spawn mailbox (sprintf "fileReaderActor_%s"  mailbox.Self.Path.Name) (FileReaderActor writer)
                paths |> Array.iter (fun path ->                    
                    //sync any way      
                    //could do same as for batches
                    let task = fileReader <? ReaderFileRead path              
                    Async.RunSynchronously task
                )
            | _ ->
                //stop
                return! reader()
        }

    reader()


//Dispatch tasks to Reader actor
let ReaderActor (writer: IActorRef) (mailbox: Actor<ReaderMessage>)  = 
   
    let rec reader() = 
        actor {
            
            let! message = mailbox.Receive()

            match message with
            | ReaderStart dir ->
                let path, ext = 
                    match dir with
                    | DirPath path -> path, ""
                    | DirPathFilter(path, filter) -> (path, "." + filter)

                let batches = 
                    System.IO.Directory.GetFiles(path, ext, IO.SearchOption.AllDirectories)             
                    |> Seq.chunkBySize INPUT_FILES_BATCH_SIZE                

                batches |> Seq.iteri (fun i batch ->
                    let batchReader = spawn mailbox (sprintf "batchReaderActor_%i"  i) (BatchReaderActor writer)
                    batchReader <! ReaderBatchRead batch
                    )

                return! waitComplete <| Seq.length batches
            | _ ->
                return! reader()
        }
    and waitComplete (batchesCount: int) = 
        actor {
            let! message = mailbox.Receive()
            //TODO catch errors in supervisions
            match message with
            | ReaderBatchReadComplete ->
                let notCompletedBatches = batchesCount - 1
                if notCompletedBatches > 0 then
                    return! waitComplete(notCompletedBatches)
                else 
                    //Stop writer and stop actor
                    writer <! WriterStop
            | _ ->
                return! reader();
        }

    reader()
