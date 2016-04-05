module utils

open RProvider
open RProvider.graphics

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255

let showImage (pixels: single array) = 
    let width, height = IMAGE_SIZE
    let pixels = pixels |> Array.map (fun f -> (float)f) |> Array.rev
    let mx = namedParams [ "data", box pixels; "nrow", box width; "ncol", box height; ] |> R.matrix 
    namedParams ["x", box mx; "xlab", box ""; "ylab", box ""; "axes", box false] |> R.image |> ignore

    //namedParams [ "side", box "x"; "labels", box false; ] |> R.axis |> ignore


let setPar (mfrow: int array) = 
    //printf "%i" pixels.Length
    //printf "%A" pixels
    let mar= [|0.;0.;0.;0.|]
    
    namedParams [ "mfrow", box mfrow;  "mar", box mar;] |> R.par |> ignore

let showImages (images: single[][]) = 
    let width, height = IMAGE_SIZE
    let n = images.Length
    let n = if images.Length <= 2 then 2 else n / 2 + n % 2;
    setPar [|n; n|]
    images |> Array.iter (fun f -> f |> showImage)
    

//http://stackoverflow.com/questions/687261/converting-rgb-to-grayscale-intensity
let inline getGrayScale ((R: _), (G: _), (B: _)) = 
    (single) (0.2126 * (float)R + 0.7152 * (float)G + 0.0722 * (float)B)
