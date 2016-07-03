module ShowImages

open image
open System.IO

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let showImages count path   =
    readLines path
    |> Seq.take count
    |> Seq.map (fun f -> 
        f.[1..] 
        |> Seq.map ( fun m -> if m = '0' then (byte) 0 else (byte) 1)
        |> Seq.toArray
    )
    |> Seq.toArray
    |> showImagesBW
    