// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open ML.Math.RunModel
open ML.Math

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    runModelFromFile 9 SoftmaxRegression.runModel @"C:\dev\.data\notMNIST_small_set\notMNIST_train.set"
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
