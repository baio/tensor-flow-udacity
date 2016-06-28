// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Akka.FSharp
open Akka.FSharp.Spawn
open Akka.Actor

[<EntryPoint>]
let main argv = 
    
    let myActorSystem = System.create "MyActorSystem" (Configuration.load ())    
    let ioActor = spawn myActorSystem "ioActor" (IORouterActor.IORouterActor)
    let inputActor = spawn myActorSystem "inputActor" (InputActor.inputBasketActor ioActor)

    inputActor <! InputActor.InputCommand.Start
    
    myActorSystem.WhenTerminated.Wait()

    0 // return an integer exit code
