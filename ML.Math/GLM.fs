module ML.Math.LinearRegression
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
}

//Given hypothesis function return Sum Square Error Function
let GenSSELossAngGradient (hypFunc: HypothesisFunc)  =
    
    let SSELoss (weights: float array) (inputs : float[] array) (outputs : float array) = 
        let calcHyp i = (hypFunc weights inputs.[i]) - outputs.[i]
        let loss = 
            inputs
            |> Array.mapi (fun i _ -> System.Math.Pow(calcHyp i, 2.) ) 
            |> Array.sum
        loss / 2.
        
    let SSEGradient (weights: float array) (features : float[] array) (labels : float array) =
        let calcHyp i = (hypFunc weights features.[i]) - labels.[i]
        weights
        |> Array.mapi (fun j _ ->         
            features 
            |> Array.mapi (fun i _ -> features.[i].[j] * (calcHyp i))            
            |> Array.sum
         )  

    SSELoss, SSEGradient

// returns true, weights - if error threshold achieved
// fales, weights - if max number of iterations achieved
let trainRegressionIterative
    (model: GLMModel) 
    (prms: IterativeTrainModelParams) 
    (inputs : float[] array) 
    (outputs : float array) =                      
        
        let biasInputs = inputs |> Array.map (fun f -> Array.concat([f; [|1.|]]))   
        
        let rec iter weights iterCnt =
            let error = model.Loss weights biasInputs outputs
            if error <= prms.MinErrorThreshold then
                true, weights
            else if prms.MaxIterNumber < iterCnt then
                false, weights
            else 
                let gradients = model.Gradient weights biasInputs outputs
                let updatedWeights = gradients |> Array.mapi (fun i w -> w + gradients.[i])
                iter updatedWeights (iterCnt + 1)
                
        // initialize random weights
        let rnd = new System.Random()
        let gauss () = nextGaussianStd rnd
        let initialWeights = Array.init biasInputs.[0].Length (fun _ -> gauss())                
        
        iter initialWeights 0
            
//input - just features
//weights - weights for features + one for bias
let predictOutput (hyp: HypothesisFunc) (weights: float[]) (input: float[]) =
    let biasInput = Array.concat([input; [|1.|]])
    hyp weights biasInput 
    
