module DataProccessing.ReadImages

open System.IO

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

/// Read images from data file 
let getImagesBytes path   =
    readLines path
    |> Seq.map (fun f -> 
        f.[1..] 
        |> Seq.map ( fun m -> if m = '0' then (byte) 0 else (byte) 1)
        |> Seq.toArray
    )
    