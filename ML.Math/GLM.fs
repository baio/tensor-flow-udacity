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
    
    let SSELoss (weights: float array) (features : float[] array) (labels : float array) = 
        let calcHyp i = (hypFunc weights features.[i]) - labels.[i]
        let loss = 
            features 
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

let hypLinear (weights: float[]) (features: float[]) = 
     weights |> Array.mapi (fun i w -> w * features.[i] )

// returns true, weights - if error threshold achieved
// fales, weights - if max number of iterations achieved
let trainIterativeModel 
    (model: GLMModel) 
    (prms: IterativeTrainModelParams) 
    (features : float[] array) 
    (labels : float array) =                      
        
        let rec iter weights iterCnt =
            let error = model.Loss weights features labels
            if error <= prms.MinErrorThreshold then
                true, weights
            else if prms.MaxIterNumber < iterCnt then
                false, weights
            else 
                let gradients = model.Gradient weights features labels
                let updatedWeights = gradients |> Array.mapi (fun i w -> w + gradients.[i])
                iter updatedWeights (iterCnt + 1)
                
        // initialize random weights
        let rnd = new System.Random()
        let gauss () = nextGaussianStd rnd
        let initialWeights = Array.init features.[0].Length (fun _ -> gauss())                
        
        iter initialWeights 0
            


    
