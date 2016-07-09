// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Akka.FSharp
open Akka.FSharp.Spawn
open Akka.Actor

open ML.Actors.Types
open ML.Actors.Init
open ML.Actors.InputActor
open ML.Actors.Image.ReadWriteCoordinatorActor
open ML.Actors.Charts.ChartActor


[<EntryPoint>]
let main argv = 

    InitLogger "http://localhost:5341"
    
    let system = System.create "Images2DataSetActorSystem" (Configuration.load ())
    let chartActor = spawn system "ChartActor" (ChartActor)    

    let rwActor = spawn system "ReadWriteCoordinator" (ReadWriteCoordinatorActor)
    let inputActor = spawn system "Input" (InputActor rwActor)
   
    let mainActor = spawn system "main" ( actorOf( fun msg ->
                    
        match msg with 
        | RWClosed path ->
            chartActor <! ChartShow {ChartType = R; DataPath = path; DataCount = 36 }
        | _ -> ()
            
    ) )

    system.EventStream.Subscribe(mainActor, typedefof<RWMessage>) |> ignore
    
    inputActor <! Start
    
    system.WhenTerminated.Wait()


    0 // return an integer exit code
