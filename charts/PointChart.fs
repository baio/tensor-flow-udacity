module  simpleCharts 

open RProvider
open RProvider.graphics

let showImageBW (pixels: byte array) = 
    
    let width = int <| System.Math.Sqrt( (float) pixels.Length )
    let height = width
    let fpixels = pixels |> Array.map float
        
    let mx = namedParams [ "data", box fpixels; "nrow", box width; "ncol", box height; ] |> R.matrix
    namedParams ["x", box mx; "xlab", box ""; "ylab", box ""; "axes", box false] |> R.image |> ignore
    

let setPar (mfrow: int array) = 

    let mar= [|0.;0.;0.;0.|]
    
    namedParams [ "mfrow", box mfrow;  "mar", box mar;] |> R.par |> ignore
    

