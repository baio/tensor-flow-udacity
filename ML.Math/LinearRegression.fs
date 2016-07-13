module ML.Math.LinearHypothesis

open ML.Math.GLM

let linearHyp (weights: float[]) (features: float[]) = 
     weights |> Array.mapi (fun i w -> w * features.[i] )

let linearDerivHyp (feature: float) _ = feature
    