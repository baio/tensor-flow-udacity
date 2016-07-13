// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


open DataProccessing.Utils;
open ML.Math.GLM;
open ML.Math.LinearRegression;

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    //runModelFromFile 9 SoftmaxRegression.runModel @"C:\dev\.data\notMNIST_small_set\notMNIST_train.set"
    //let inputs, outputs = readCSV @"c:/dev/.data/faithful.csv" true [|1|] 2
    //y = 0.5 x 
    //w1 = 0.5; w2 = 0.
    let inputs = [|
        [|0.|]
        [|1.|]
        [|2.|]
        [|3.|]
     |]
    let outputs = [|
        0.
        1.
        2.
        4.
     |]
    let model = {
        Hypothesis = linearHyp;
        Loss = SSELoss linearHyp;
        Gradient = SSEGradient linearHyp;
    }
    let prms = {
        MaxIterNumber = 10000;
        MinErrorThreshold = 0.00001;
        StepSize = 0.05;
    }
    let ws = trainGradientDescent model prms inputs outputs
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
