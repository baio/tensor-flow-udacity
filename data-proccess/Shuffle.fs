module DataProccessing.Shuffle

open Utils
open Nessos.FsPickler

//http://www.clear-lines.com/blog/post/Optimizing-some-old-F-code.aspx
let shuffle (rng: System.Random) (length: int) (items : seq<_>)  =
   let rec shuffleTo (indexes: int[]) upTo =
      match upTo with
      | 0 -> indexes
      | _ ->
         let fst = rng.Next(upTo)
         let temp = indexes.[fst]
         indexes.[fst] <- indexes.[upTo] 
         indexes.[upTo] <- temp
         shuffleTo indexes (upTo - 1)
   let indexes = [| 0 .. length - 1 |]
   let shuffled = shuffleTo indexes (length-1)
   Seq.permute (fun i -> shuffled.[i]) items


let shuffleSetFileSeq linesNumber setFile  =        
    shuffle (new System.Random()) linesNumber (readLines setFile)


let shuffleSetFile linesNumber inFile outFile =
    writeLines outFile (shuffleSetFileSeq linesNumber inFile)
