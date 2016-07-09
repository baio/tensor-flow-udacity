// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Akka.FSharp
open Akka.FSharp.Spawn
open Akka.Actor
open System.Drawing
open ShowImages

[<EntryPoint>]
let main argv = 
    
    
    let myActorSystem = System.create "MyActorSystem" (Configuration.load ())    
    let ioActor = spawn myActorSystem "ioActor" (IORouterActor.IORouterActor)
    let inputActor = spawn myActorSystem "inputActor" (InputActor.inputBasketActor ioActor)

    inputActor <! InputActor.InputCommand.Start
    
    myActorSystem.WhenTerminated.Wait()
    
    //showImages 36 @"C:\dev\.data\notMNIST_normalized.csv"

                     
    0 // return an integer exit code
