module ML.Math.BatchGradientDescent

open ML.Math.GLM


// returns true, weights - if error threshold achieved
// fales, weights - if max number of iterations achieved
let batchGradientDescent
    (model: GLMModel) 
    (prms: IterativeTrainModelParams) 
    (inputs : float[] array) 
    (outputs : float array) =                      
        
        let biasInputs = inputs |> Array.map (fun f -> Array.concat([f; [|1.|]]))   
        
        let rec iter weights iterCnt latestError =
            let error = model.Loss weights biasInputs outputs
            if latestError = error then
                // no improvements, converged
                Converged, weights
            else if error <= prms.MinErrorThreshold then
                // got minimal error threshold
                ErrorThresholdAchieved, weights
            else if prms.MaxIterNumber < iterCnt then
                // iters count achieved
                MaxIterCountAchieved, weights
            //TODO : or not become better
            else 
                let gradients = model.Gradient weights biasInputs outputs                
                //If graient is plus thwn we need to move down to achive function min
                let updatedWeights = 
                    weights 
                    |> Array.mapi (fun i w -> w - prms.StepSize * gradients.[i])
                //printfn "%i: \n grads: %A \n weights : %A" iterCnt gradients updatedWeights
                iter updatedWeights (iterCnt + 1) error
                
        // initialize random weights
        let initialWeights = Array.zeroCreate biasInputs.[0].Length         
        iter initialWeights 0 0.
