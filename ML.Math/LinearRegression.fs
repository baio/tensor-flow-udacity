module ML.Math.LinearRegression

open ML.Math.GLM

let linearHyp (weights: float[]) (features: float[]) = 
     weights |> Array.mapi (fun i w -> w * features.[i] ) |> Array.sum

    