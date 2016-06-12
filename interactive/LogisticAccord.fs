module LogisticAccord

open Accord.IO
open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting


type InputItem =  float[] * int 

let mapCSVLine (line : string array) : InputItem = 
    let parsed = line |> Array.map System.Double.Parse
    parsed.[1..], int parsed.[0]
       
let readCSV path = 
    use file = new System.IO.StreamReader(new System.IO.FileStream(path, System.IO.FileMode.Open))
    let csv = new CsvReader(file, true)
    let lines = csv.ReadToEnd();
    lines |> List.ofSeq |> List.map mapCSVLine

let runModel (inputsOutputs: InputItem list) = 
    let inputs, outputs = inputsOutputs |> Array.ofList |> Array.unzip    

    // To verify this hypothesis, we are going to create a logistic
    // regression model for those two inputs (age and smoking).
    let regression = new LogisticRegression(inputs.[0].Length)

    // Next, we are going to estimate this model. For this, we
    // will use the Iteratively Reweighted Least Squares method.
    let teacher = new IterativeReweightedLeastSquares(regression)

    // Now, we will iteratively estimate our model. The Run method returns
    // the maximum relative change in the model parameters and we will use
    // it as the convergence criteria.

    let mutable delta = System.Double.MaxValue
    while (delta > 0.001) do
        // Perform an iteration
        delta <- teacher.Run(inputs, outputs)

    printfn "%A" regression.Coefficients
    printfn "%A" regression.StandardErrors

let TestLogisticModel path = 
    
    let inputsOutputs = readCSV path

    runModel inputsOutputs

