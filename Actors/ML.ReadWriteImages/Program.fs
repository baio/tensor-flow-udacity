// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Akka.FSharp
open Akka.FSharp.Spawn
open Akka.Actor
open FSharp.Configuration

open DataProccessing.Measures
open DataProccessing.Types
open DataProccessing.Shuffle
open DataProccessing.Split
open DataProccessing.Utils
open ML.Actors.Types
open ML.Actors.Init
open ML.Actors.InputActor
open ML.Actors.Image.ReadWriteCoordinatorActor
open ML.Actors.Charts.ChartActor
open ML.Actors.SplitSetActor

type Settings = AppSettings<"app.config">

let tvtSizes = {
    train = 70<percent>; 
    validation = 15<percent>; 
    test = 15<percent>
}
    
let tvtPaths = { 
    train = Settings.ML_IMAGES_OUTPUT_TRAIN_FILE_PATH; 
    validation = Settings.ML_IMAGES_OUTPUT_VALID_FILE_PATH; 
    test = Settings.ML_IMAGES_OUTPUT_TEST_FILE_PATH; 
}

[<EntryPoint>]
let main argv = 

    InitLogger Settings.SeqUri.OriginalString
    
    let system = System.create "Images2DataSetActorSystem" (Configuration.load ())
    let chartActor = spawn system "ChartActor" (ChartActor)    

    let rwActor = spawn system "ReadWriteCoordinator" (ReadWriteCoordinatorActor)
    let splitSetActor = spawn system "SplitSetActor" (SplitSetActor)
      
    let mainActor = spawn system "main" ( actorOf2( fun mailbox msg ->
                    
        match box msg with 
        | :? RWMessage as rw ->
            match rw with
            | RWClosed(cnt, path) ->                            
                async {             
                    return! splitSetActor <? 
                        TVTSplitSet({ InputFilePath = path; InputSamplesCount = cnt; Output = {Sizes = tvtSizes; Paths = tvtPaths} }) 

                } |!> mailbox.Self                
            | _ -> ()
        | :? TVTSplitSetCommands as st ->
            match st with 
            | TVTSplitSetComplete ->                
                printfn "finished"
                chartActor <! ChartShow {ChartType = R; DataPath = Settings.ML_IMAGES_OUTPUT_TRAIN_FILE_PATH; DataCount = 36 }
                System.Console.ReadKey() |> ignore
                system.Terminate() |> ignore
            | _ -> ()
        | _ -> ()
            
    ) )

    system.EventStream.Subscribe(mainActor, typedefof<RWMessage>) |> ignore
    
  
    rwActor <! RWStart {
        input = DirPath Settings.ML_IMAGES_INPUT_DIR_PATH; 
        output = Settings.ML_IMAGES_OUTPUT_FILE_PATH
        }
   
    //mainActor <! RWClosed(10000, Settings.ML_IMAGES_OUTPUT_FILE_PATH)
    
    system.WhenTerminated.Wait()

    0 // return an integer exit code
