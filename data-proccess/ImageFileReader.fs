module DataProccessing.ImageFileReader

open Types
open Utils
open Maybe

open System.Drawing

let flat2dArray (arr : 'a[,]) : 'a[]=
    //2d array -> array of arrays
    Array.init ( arr.GetLength(1) ) (fun i -> arr.[*, i])  
    //flat array of arrays
    |> Array.collect (fun f -> f) 

let getPixelRGB (bitmap : Bitmap) (i: int) (j: int)  = 
    let pixel = bitmap.GetPixel(i, j)
    pixel.R, pixel.G, pixel.B    


let getPixelGrey (bitmap : Bitmap) (i: int) (j: int)  = 
    //http://stackoverflow.com/questions/687261/converting-rgb-to-grayscale-intensity
    // B & W 
    let r, g, b = getPixelRGB bitmap i j
    (single) (0.2126 * (float)r + 0.7152 * (float)g + 0.0722 * (float)b)

let getPixelBW (bitmap : Bitmap) (i: int) (j: int) = 
    let grey = getPixelGrey bitmap i j
    if grey < single 10 then
        byte 0
    else
        byte 1
  
// Read iamge and return greyscaled pixels
let readImage (imageSize : ImageSize) (path: string) : ImageInBW option = 
    
    try
        let bitmap = new Bitmap(path)
     
        if imageSize <> { width = bitmap.Size.Width; height = bitmap.Size.Height}  then 
            None
        else 
            Array2D.zeroCreate<int> imageSize.width imageSize.height 
            |> Array2D.mapi (fun i j _ -> getPixelBW bitmap i j)
            |> Some
    with 
    | _ -> None

// Read all images from directory
let readImages (imageSize : ImageSize) (dirPath: DirPath) (iamgePath2Label : string -> string) : LabeledImage seq = 
    
    getFilePaths dirPath
    |> Seq.choose (fun path ->                                
        maybe {
            let! image = readImage imageSize path
            return { label = (iamgePath2Label path); image = image }
        }
    )    




