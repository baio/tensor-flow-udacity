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
    | ReaderBatchRead of string array
    | ReaderFileRead of string   
    | ReaderFileReadComplete
    | ReaderBatchReadComplete
    // stop read
    | ReaderStop

let mapPath2Label (path: string) = 
     (new System.IO.FileInfo(path)).Directory.Name

let FileReaderActor (writer: IActorRef) (mailbox: Actor<ReaderMessage>) = 
               
    let rec reader() = 
        actor {
    
            let! msg = mailbox.Receive()
            let parent = mailbox.Context.Parent

            match msg with 
            | ReaderFileRead path -> 
                maybe {
                    let! image = readImage {width = 28; height = 28} path
                    let bytes = flat2dArray image
                                    |> Array.map string
                                    |> System.String.Concat                    
                    writer <! WriterWrite ((mapPath2Label path) + bytes)
                } |> ignore
                parent <! ReaderFileReadComplete
                return! reader()
            | _ -> 
                return! reader()

            parent <! ReaderFileReadComplete
            return! reader()                
        }

    reader()

let BatchReaderActor (writer: IActorRef) (mailbox: Actor<ReaderMessage>) = 

    mailbox.Defer ( fun () -> 
        mailbox.Sender() <! ReaderBatchReadComplete )

    let rec reader() = 
        actor {
            
            let! message = mailbox.Receive()

            match message with
            | ReaderFileReadComplete ->
                mailbox.Stash()
            | ReaderBatchRead paths ->                
                let fileReader = spawn mailbox (sprintf "fileReaderActor_%s"  mailbox.Self.Path.Name) (FileReaderActor writer)
                paths |> Array.iter (fun path ->                    
                    async {
                        do! fileReader <? (ReaderFileRead path)
                    } |!> mailbox.Self
                )
                mailbox.UnstashAll()
                return! waitComplete(paths.Length)
            | _ ->
                return! reader()
        }
    and waitComplete(waitForCnt) =
        
        actor {
                    
            let! message = mailbox.Receive()
            
            match message with
            | ReaderFileReadComplete ->
                if waitForCnt - 1 > 0 then                    
                    return! waitComplete(waitForCnt - 1)
                else 
                    mailbox.Context.Parent <! ReaderBatchReadComplete
            | _ -> 
                return! waitComplete(waitForCnt)
                
        }

    reader()


//Dispatch tasks to Reader actor
let ReaderActor (writer: IActorRef) (mailbox: Actor<ReaderMessage>)  = 
   
    let rec reader() = 
        actor {
            
            let! message = mailbox.Receive()

            match message with
            | ReaderStart dir ->
                let path, filter = 
                    match dir with
                    | DirPath path -> path, "*"
                    | DirPathFilter(path, filter) -> (path, "*." + filter)

                let batches = 
                    System.IO.Directory.GetFiles(path, filter, IO.SearchOption.AllDirectories)             
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
                printfn "Stop batch reader (left %i)" notCompletedBatches
                if notCompletedBatches > 0 then
                    return! waitComplete(notCompletedBatches)
                else 
                    //Stop writer and stop actor
                    writer <! WriterStop
            | _ ->
                return! reader();
        }

    reader()
