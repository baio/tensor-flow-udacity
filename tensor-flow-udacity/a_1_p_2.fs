module a_1_p_2

open utils


(*
Problem 2
Let's verify that the data still looks good. Displaying a sample of the labels and images from the ndarray. Hint: you can use matplotlib.pyplot.
*)

open System.IO // Name spaces can be opened just as modules

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255


let getRGBfrom12bytes bytes =
    match bytes with
        | [ r1; r2; r3; r4; g1; g2; g3; g4; b1; b2; b3; b4 ] -> 
            let R : single array = [|(single)0.|]
            let G : single array = [|(single)0.|]
            let B : single array = [|(single)0.|]
            System.Buffer.BlockCopy([|r1; r2; r3; r4|], 0, R, 0, 4)
            System.Buffer.BlockCopy([|g1; g2; g3; g4|], 0, G, 0, 4)
            System.Buffer.BlockCopy([|b1; b2; b3; b4|], 0, B, 0, 4)
            R.[0], G.[0], B.[0]
        | _ ->  failwith "Array has wrong size"           

let getPixelfrom12bytes bytes =
    bytes |> getRGBfrom12bytes |> getGrayScale


///
/// Bytes R-4 bytes, G-4 bytes, B-4 bytes -> float, float, float (normalized by 255)
let getPixels bytes = 
    bytes 
    |> Seq.chunkBySize 12
    |> Seq.map(fun m -> m |> Array.toList |> getPixelfrom12bytes )


let readFileAsBytesArray sampleSize fileName =     
    let width, height = IMAGE_SIZE
    let ar = Array.zeroCreate(width * height * 12 * sampleSize)
    let fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
    try        
        fileStream.Read(ar, 0, ar.Length) |> ignore
    finally
        fileStream.Close()
    ar

let readFileAndGetImages sampleSize fileName  =     
    let width, height = IMAGE_SIZE
    readFileAsBytesArray sampleSize fileName 
    |> getPixels
    |> Seq.chunkBySize (width * height)
    |> Seq.map Seq.toArray
    |> Seq.toArray    

let readFileAndShowImages sampleSize fileName =     
    readFileAndGetImages sampleSize fileName 
    |> showImages

let readDirectoryAndShowImages sampleSize dirName =
    dirName 
    |> System.IO.Directory.EnumerateFiles
    |> Seq.collect (fun f -> 
        readFileAndGetImages sampleSize f)
    |> Seq.toArray
    |> showImages


    