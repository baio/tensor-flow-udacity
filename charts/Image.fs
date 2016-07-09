module ML.Charts.R.Image

open RProvider
open RProvider.graphics


let setPar (mfrow: int array) = 

    let mar= [|0.;0.;0.;0.|]
    
    namedParams [ "mfrow", box mfrow;  "mar", box mar;] |> R.par |> ignore
    

let revImage dim pixels = 
    pixels 
    |> Seq.chunkBySize dim
    |> Seq.rev    
    |> Seq.collect (fun f -> f)
    |> Seq.toArray

let showImageBW (pixels: byte array) = 
    
    let width = int <| System.Math.Sqrt( (float) pixels.Length )
    let height = width
    let fpixels = pixels |> Array.map float |> revImage width
        
    let mx = namedParams [ "data", box fpixels; "nrow", box width; "ncol", box height; ] |> R.matrix
    namedParams ["x", box mx; "xlab", box ""; "ylab", box ""; "axes", box false] |> R.image |> ignore
    

let showImagesBW (images: byte[][]) = 
    let l = System.Math.Sqrt(float images.Length)
    let pars = (int  l) + (if l = System.Math.Floor(l) then 0 else 1)
    setPar [|pars; pars|]
    images |> Array.iter showImageBW
    
