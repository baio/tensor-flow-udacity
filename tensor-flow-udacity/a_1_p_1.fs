module a_1_p_1
(*
Problem 1
Let's take a peek at some of the data to make sure it looks sensible. Each exemplar should be an image of a character A through J rendered in a different font. Display a sample of the images that we just downloaded. Hint: you can use the package IPython.display.
Now let's load the data in a more manageable format. Since, depending on your computer setup you might not be able to fit it all in memory, we'll load each class into a separate dataset, store them on disk and curate them independently. Later we'll merge them into a single dataset of manageable size.
We'll convert the entire dataset into a 3D array (image index, x, y) of floating point values, normalized to have approximately zero mean and standard deviation ~0.5 to make training easier down the road.
A few images might not be readable, we'll just skip them.
*)


open System.Drawing;

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255
let FILE_EXT = "bl" //which means binary letter

let normalizePixelColor(pixel: byte) =   
    single (pixel - (byte) PIXEL_DEPTH / (byte 2)) / single PIXEL_DEPTH

let normalizePixelColors(pixel : byte * byte * byte) =   
    let r, g, b = pixel;
    normalizePixelColor(r), normalizePixelColor(g), normalizePixelColor(b)
    
let getPixels (bmp:Bitmap) =  
        
  let mapPixel i j _  = 
    let pixel = bmp.GetPixel(i, j)
    pixel.R, pixel.G, pixel.B    
  
  Array2D.zeroCreate<int> bmp.Width bmp.Height
  |> Array2D.mapi mapPixel
  |> Array2D.map normalizePixelColors

let loadBitmap(fileName: System.String) =  
    
    let bitmap = new Bitmap(fileName)
      
    if IMAGE_SIZE <> (bitmap.Size.Width, bitmap.Size.Height)  then 
      raise(new System.Exception("Image size don't match"))
  
    getPixels bitmap


let flatSingleImagePixels (pixels : (_ * _ * _)[,]) =
    //2d array -> array of arrays
    Array.init ( pixels.GetLength(1) ) (fun i -> pixels.[*, i])  
    //flat array of arrays
    |> Array.collect (fun f -> f) 
    //flat array of tulpes
    |> Array.collect (fun (r, g, b) -> [|r; g; b|])

let flatImagesPixels(pixels : (_ * _ * _)[,][]) =
    pixels  
    //array of images pixels
    |> Array.map flatSingleImagePixels 
    //flat this array
    |> Array.collect (fun f -> f)

/// <summary>
/// Flat image pixels represented as floats from 2d array of r,g,b tulpes into sequency of bytes.
/// 4 bytes conatin one float.
/// Then write to the file.
/// </summary>
/// <param name="path">File where to write flattened adat</param>
/// <param name="images">Array of 2d arrays with r, g, b tulpes</param>
let writeBinary (path: string) (images : (single * single * single)[,][]) =    
    //http://stackoverflow.com/questions/6397235/write-bytes-to-file
    //http://stackoverflow.com/questions/4635769/how-do-i-convert-an-array-of-floats-to-a-byte-and-back
    let floats = flatImagesPixels images
    let bytes : byte array = Array.zeroCreate (floats.Length * 4)
    System.Buffer.BlockCopy(floats, 0, bytes, 0, bytes.Length)
    let fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write)
    try        
        fileStream.Write(bytes, 0, bytes.Length)
    finally
        fileStream.Close()


let readLetterFolder folder =
    System.IO.Directory.EnumerateFiles(folder) 
    |> Seq.map loadBitmap
    |> Seq.toArray

let getFolderLetter (folder : string) =
    folder.[folder.Length - 1].ToString()

let getOutPath letterPath outFolder =     
    System.IO.Path.Combine([|outFolder; (getFolderLetter(letterPath) + "." + FILE_EXT)|])

let createAlphabitBinaries alphabitFolderName outFolderName = 
    System.IO.Directory.EnumerateDirectories(alphabitFolderName)
    |> Seq.map (fun folder -> folder, readLetterFolder folder)
    |> Seq.iter (fun (path, images) -> writeBinary (getOutPath path outFolderName) images ) 
