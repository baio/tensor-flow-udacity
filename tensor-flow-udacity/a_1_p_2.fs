module a_1_p_2

open utils


(*
Problem 2
Let's verify that the data still looks good. Displaying a sample of the labels and images from the ndarray. Hint: you can use matplotlib.pyplot.
*)

open System.IO // Name spaces can be opened just as modules

let IMAGE_SIZE = 28 , 28
let PIXEL_DEPTH = 255



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

let readFileAndShowImages showImages sampleSize fileName =     
    readFileAndGetImages sampleSize fileName 
    |> showImages

let readDirectoryAndShowImages showImages sampleSize dirName =
    dirName 
    |> System.IO.Directory.EnumerateFiles
    |> Seq.collect (fun f -> 
        readFileAndGetImages sampleSize f)
    |> Seq.toArray
    |> showImages


    