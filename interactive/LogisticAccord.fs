module LogisticAccord

open utils
open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting
//open Accord.Statistics.Models.Regression.Linear

let calcAccuracy (regression : GeneralizedLinearRegression) (inputs : float[][]) (outputs : int[]) : float =    

    let folder acc (input : float[]) (expectedOutput : int) = 
        let actualProbabiltyOutput = regression.Compute(input)
        let actualOutput = if actualProbabiltyOutput < 0.5 then 0 else 1
        match actualOutput = expectedOutput with 
            | true -> acc + 1.
            | false -> acc

    let correctlyClassified = Array.fold2 folder 0. inputs outputs
    correctlyClassified / float outputs.Length
    
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

    let accuracy = calcAccuracy regression inputs outputs

    printfn "%f" accuracy
    printfn "%A" regression.Coefficients
    printfn "%A" regression.StandardErrors

let testLogisticModel path mapLine = 
    
    let inputsOutputs = readCSV path mapLine

    runModel inputsOutputs 

