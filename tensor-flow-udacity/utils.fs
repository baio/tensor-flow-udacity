module utils

open RProvider
open RProvider.graphics

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255

let showImage (pixels: float array) = 
    printf "%i" pixels.Length
    printf "%A" pixels
    let width, height = IMAGE_SIZE
    let floatPixels = pixels
    let mx = namedParams [ "data", box floatPixels; "nrow", box width; "ncol", box height; ] |> R.matrix
    R.image mx 


//http://stackoverflow.com/questions/687261/converting-rgb-to-grayscale-intensity
let getGrayScale ((R: single), (G: single), (B: single)) = 
    (single) (0.2126 * (float)R + 0.7152 * (float)G + 0.0722 * (float)B)
