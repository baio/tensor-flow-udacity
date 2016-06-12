// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LogisticAccord
open MultinominalLogisticAccord

let mapCSVLine1 line = 
    let parsed = line |> Array.map System.Double.Parse
    parsed.[1..], int parsed.[0]
   
let mapCSVLine2 (line : string array) = 
    let useFeatures = [|5;3;7|] |> Array.map (fun f -> line.[f])
    let parsed = useFeatures |> Array.map System.Double.Parse
    parsed.[1..], int parsed.[0]

[<EntryPoint>]
let main argv = 

    testMultinominalLogisticModel [|3; 1; 2|] @"C:\dev\tensor-flow-udacity\data\examples\multi.csv" mapCSVLine2

    System.Console.ReadKey() |> ignore;
    0 // return an integer exit code