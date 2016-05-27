module tensor_flow_udacity.r.utils

open RProvider
open RProvider.graphics

let showImage imageSize (pixels: single array) = 
    
    let width, height = imageSize
    let pixels = pixels |> Array.map (fun f -> (float)f) //|> Array.rev
    
    let mx = namedParams [ "data", box pixels; "nrow", box width; "ncol", box height; ] |> R.matrix
    namedParams ["x", box mx; "xlab", box ""; "ylab", box ""; "axes", box false] |> R.image |> ignore

    //namedParams [ "side", box "x"; "labels", box false; ] |> R.axis |> ignore


let setPar (mfrow: int array) = 
    //printf "%i" pixels.Length
    //printf "%A" pixels
    let mar= [|0.;0.;0.;0.|]
    
    namedParams [ "mfrow", box mfrow;  "mar", box mar;] |> R.par |> ignore
    

