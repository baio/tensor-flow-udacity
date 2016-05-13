module utils

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255

let showImages setPar showImage (images: single[][])  = 
    let n = images.Length    
    let n = if n <= 2 then 2 else System.Convert.ToInt32(System.Math.Round(System.Math.Sqrt((float)n))) + 1;
    printf "length %i" n
    setPar [|n; n|]
    images |> Array.iter (fun f -> f |> showImage IMAGE_SIZE)
    

//http://stackoverflow.com/questions/687261/converting-rgb-to-grayscale-intensity
let inline getGrayScale ((R: _), (G: _), (B: _)) = 
    (single) (0.2126 * (float)R + 0.7152 * (float)G + 0.0722 * (float)B)

let inline RGB2GrayScale (arr : _ array) = getGrayScale (arr.[0], arr.[1], arr.[2])