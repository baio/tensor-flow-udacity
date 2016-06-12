module MultinominalLogisticAccord

open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting

open utils

let calcAccuracy (cats : int array) (regression : MultinomialLogisticRegression) (inputs : float[][]) (outputs : int[]) : float =    

    let folder acc (input : float[]) (expectedOutput : int) = 

        let actualProbabiltyOutput = regression.Compute(input)

        //returns pair - max probabilty value & its index (0 index -> 1st encounterd category, 1 index -> 2nd, etc ...)
        let (_, maxProbabiltyCategoryIndex), _ = 
            actualProbabiltyOutput
            |> Array.fold (fun ((acc, i), ir) v -> if acc < v then (v, ir), ir + 1 else (acc, i), ir + 1) ((0., 0), 0)
        
        let actualOutput = cats.[maxProbabiltyCategoryIndex];

        //printfn "index = %i : actual = %i : expected = %i (%A)" maxProbabiltyCategoryIndex actualOutput expectedOutput actualProbabiltyOutput

        match actualOutput = expectedOutput with 
            | true -> acc + 1.
            | false -> acc

    let correctlyClassified = Array.fold2 folder 0. inputs outputs
    correctlyClassified / float outputs.Length

let runModel (cats : int array) (inputsOutputs: InputItem list) = 
    
    let inputs, outputs = inputsOutputs |> Array.ofList |> Array.unzip    
    
    // Create a new Multinomial Logistic Regression for 3 categories
    let mlr = new MultinomialLogisticRegression(inputs.[0].Length, cats.Length)

    // Create a estimation algorithm to estimate the regression
    let lbnr = new LowerBoundNewtonRaphson(mlr)

    // Now, we will iteratively estimate our model. The Run method returns
    // the maximum relative change in the model parameters and we will use
    // it as the convergence criteria.

    let mutable delta = System.Double.MaxValue
    let mutable  iteration = 0

    while (iteration < 100 && delta > 1e-6) do   
        // Perform an iteration
        delta <- lbnr.Run(inputs, outputs);
        iteration <- iteration + 1
    
    //(Array.take 10 inputs)
    let accuracy = calcAccuracy cats mlr inputs outputs

    printfn "%f" accuracy
    printfn "%A" mlr.Coefficients
    printfn "%A" mlr.StandardErrors

let testMultinominalLogisticModel cats path mapLine = 
    
    let inputsOutputs = readCSV path mapLine 

    runModel cats inputsOutputs
