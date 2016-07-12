module ML.Math.SoftmaxRegression

open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting

let runModel (numberOfClasses : int) (samples : int[] * float[][]) = 
        
    let labels, features = samples
    // Create a new Multinomial Logistic Regression for 3 categories
    let mlr = new MultinomialLogisticRegression(features.[0].Length, numberOfClasses)
    
    // Create a estimation algorithm to estimate the regression
    let lbnr = new LowerBoundNewtonRaphson(mlr)
    //Accord.Statistics.Models.Regression.Fitting.IterativeReweightedLeastSquares(ml)

    // Now, we will iteratively estimate our model. The Run method returns
    // the maximum relative change in the model parameters and we will use
    // it as the convergence criteria.

    let mutable delta = System.Double.MaxValue
    let mutable  iteration = 0

    while (iteration < 100 && delta > 1e-6) do   
        // Perform an iteration
        delta <- lbnr.Run(features, labels)
        iteration <- iteration + 1
    
    //printfn "%f" accuracy
    printfn "%A" mlr.Coefficients
    printfn "%A" mlr.StandardErrors
