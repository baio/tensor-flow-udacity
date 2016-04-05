module utils

open RProvider
open RProvider.graphics

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255

let showImage (pixels: float array) = 
    //printf "%i" pixels.Length
    //printf "%A" pixels
    let width, height = IMAGE_SIZE
    let pixels = pixels |> Array.rev
    let mx = namedParams [ "data", box pixels; "nrow", box width; "ncol", box height; ] |> R.matrix 
    mx |> R.image |> ignore


let setPar (mfrow: int array) = 
    //printf "%i" pixels.Length
    //printf "%A" pixels

    namedParams [ "mfrow", box mfrow;  ] |> R.par |> ignore

    


//http://stackoverflow.com/questions/687261/converting-rgb-to-grayscale-intensity
let inline getGrayScale ((R: _), (G: _), (B: _)) = 
    (single) (0.2126 * (float)R + 0.7152 * (float)G + 0.0722 * (float)B)
