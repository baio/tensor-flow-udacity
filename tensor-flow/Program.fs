// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Akka.FSharp
open Akka.FSharp.Spawn
open Akka.Actor
open System.Drawing
open ShowImages
open Serilog

[<EntryPoint>]
let main argv = 

    //http://localhost:5341/

    let logger = (new LoggerConfiguration()).WriteTo.Seq("http://localhost:5341").MinimumLevel.Debug().CreateLogger()
    Serilog.Log.Logger <- logger
        
    let myActorSystem = System.create "MyActorSystem" (Configuration.load ())    
    let ioActor = spawn myActorSystem "ioActor" (IORouterActor.IORouterActor)
    let inputActor = spawn myActorSystem "inputActor" (InputActor.inputBasketActor ioActor)

    inputActor <! InputActor.InputCommand.Start
    
    myActorSystem.WhenTerminated.Wait()

    System.Console.ReadKey() |> ignore
    
    //showImages 36 @"C:\dev\.data\notMNIST_normalized.csv"

                     
    0 // return an integer exit code
