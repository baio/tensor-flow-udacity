module ML.Math.GLM

open ML.Math.Utils

// GLM 
// http://ufldl.stanford.edu/tutorial/supervised/LinearRegression/

// Given weights and features return calculated label
type HypothesisFunc = float [] -> float [] -> float
// Given weights, features and labels calculate error
type LossFunc = float [] -> float [][] -> float[] -> float
// Given weights, features and labels calculate gradient array for weights
type GradientFunc = float [] -> float [][] -> float[] -> float[]
// Given HypothesisFunc returns cost and gradient functions
type GenLossAndGradientFunc = HypothesisFunc -> LossFunc * GradientFunc

type GLMModel = {
    Hypothesis : HypothesisFunc
    Loss : LossFunc   
    Gradient : GradientFunc   
}

type IterativeTrainModelParams = {
    MaxIterNumber : int
    MinErrorThreshold : float
    StepSize: float
}

type ModelTrainResult = Converged | ErrorThresholdAchieved | MaxIterCountAchieved

let SSELoss (hypFunc: HypothesisFunc) (weights: float array) (inputs : float[] array) (outputs : float array) = 
    let hypError i = (hypFunc weights inputs.[i]) - outputs.[i]
    let loss = 
        inputs
        |> Array.mapi (fun i _ -> System.Math.Pow(hypError i, 2.) ) 
        |> Array.sum
    loss / 2.
        
let SSEGradient (hypFunc: HypothesisFunc) (weights: float array) (inputs : float[] array) (outputs : float array) =
    let hypError i = (hypFunc weights inputs.[i]) - outputs.[i]
    weights
    |> Array.mapi (fun j _ ->         
        inputs 
        |> Array.mapi (fun i _ -> inputs.[i].[j] * (hypError i))            
        |> Array.sum
    )  
            
//input - just features
//weights - weights for features + one for bias
let predictOutput (hyp: HypothesisFunc) (weights: float[]) (input: float[]) =
    let biasInput = Array.concat([input; [|1.|]])
    hyp weights biasInput 
    
