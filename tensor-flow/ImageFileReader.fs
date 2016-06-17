module ImageFileReader

open types
open System.Drawing;
open maybe;

let getPixelRGB (bitmap : Bitmap) (i: int) (j: int)  = 
    let pixel = bitmap.GetPixel(i, j)
    pixel.R, pixel.G, pixel.B    

let getPixelGrey (bitmap : Bitmap) (i: int) (j: int)  = 
    //http://stackoverflow.com/questions/687261/converting-rgb-to-grayscale-intensity
    let r, g, b = getPixelRGB bitmap i j
    (single) (0.2126 * (float)r + 0.7152 * (float)g + 0.0722 * (float)b)
  
// Read iamge and return greyscaled pixels
let readImage (imageSize : ImageSize) (path: string) : ImageInGreyScale option = 
    
    let bitmap = new Bitmap(path)
     
    if imageSize <> { width = bitmap.Size.Width; height = bitmap.Size.Height}  then 
        None
    else 
        Array2D.zeroCreate<int> imageSize.width imageSize.height 
        |> Array2D.mapi (fun i j _ -> getPixelGrey bitmap i j)
        |> Some


let readImages (imageSize : ImageSize) (dirPath: string) (iamgePath2Label : string -> string) : LabeledImage seq = 
    System.IO.Directory.EnumerateFiles(dirPath) 
    |> Seq.choose (fun path ->         
                
        //With maybe computational expression
        maybe {
            let! image = readImage imageSize path
            return { label = (iamgePath2Label path); image = image }
        }
        (*
        //with infix bind
        readImage imageSize path >>= (fun image -> Some { label = (iamgePath2Label path); image = image })
        *)
        (*
        // with bind
        Option.bind (fun image -> Some { label = (iamgePath2Label path); image = image }) (readImage imageSize path)
        *)
        (*
        //With option map
        Option.map (fun image -> { label = (iamgePath2Label path); image = image }) (readImage imageSize path)        
        *)
        (*
        Simple
        match readImage imageSize path with
            | Some image -> Some({ label = (iamgePath2Label path); image = image })
            | None -> None
        *)
    )    