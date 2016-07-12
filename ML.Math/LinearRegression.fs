module ML.Math.LinearHypothesis

let linearHyp (weights: float[]) (features: float[]) = 
     weights |> Array.mapi (fun i w -> w * features.[i] )
