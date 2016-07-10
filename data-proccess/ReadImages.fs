module DataProccessing.ReadImages

open Utils
open Nessos.Streams

/// Read images from data file 
let getImagesBytes path   =
    readLinesS path
    |> Stream.map (fun f -> 
        f.[1..] 
        |> Seq.map ( fun m -> if m = '0' then (byte) 0 else (byte) 1)
        |> Seq.toArray
    )
    