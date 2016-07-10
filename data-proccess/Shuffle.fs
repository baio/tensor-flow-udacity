module DataProccessing.Shuffle

open Utils
open Nessos.FsPickler

let swap (a: _[]) x y =
    let tmp = a.[x]
    a.[x] <- a.[y]
    a.[y] <- tmp

let generateShuffled (rng: System.Random) upTo =
    let arr = [|0..upTo - 1|]
    let random = new System.Random();    
    arr |> Seq.iteri (fun i _ -> swap arr i (rng.Next(i, upTo))) 
    arr
        
let shuffle (rng: System.Random) (length: int) (items : seq<_>) =
    let shuffled = generateShuffled rng length    
    items 
    |> Seq.take length
    |> Seq.permute (fun i -> shuffled.[i]) 
    
let shuffleSetFile linesNumber inFile outFile =
    readLines inFile          
    |> shuffle (new System.Random()) linesNumber
    |> writeLines outFile
