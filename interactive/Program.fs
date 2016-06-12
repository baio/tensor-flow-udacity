// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LogisticAccord

[<EntryPoint>]
let main argv = 

    TestLogisticModel @"C:\dev\tensor-flow-udacity\data\examples\logistic_ucla.csv"

    System.Console.ReadKey() |> ignore;
    0 // return an integer exit code