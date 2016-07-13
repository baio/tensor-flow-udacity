module DataProccessing.Utils

open Types
open Nessos.Streams

let rec getPathsRec (dirPath: string) : string seq =    
    System.IO.Directory.GetDirectories(dirPath)
    |> Seq.collect getPathsRec

let getFilePaths (dirPath: DirPath) : string [] =    
    
    let path, filter = 
        match dirPath with 
            | DirPath path -> path, ""
            | DirPathFilter(path, filter) -> path, filter
        
    System.IO.Directory.GetFiles(path, filter, System.IO.SearchOption.AllDirectories)

let readLines (filePath:string) = seq {
    use sr = new System.IO.StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let writeLines (filePath:string) (lines: seq<string>) = 
    use sr = new System.IO.StreamWriter (filePath, false)
    lines |> Seq.iter sr.WriteLine
    sr.Close()

let readLinesS (filePath:string) = 
    let sr = new System.IO.StreamReader (filePath)

    let eof() = 
        if sr.EndOfStream then 
            sr.Dispose()
            true
        else 
            false
    
    let rl() = 
        try
            sr.ReadLine()
        with e ->
            sr.Dispose()
            raise e

    Stream.generateInfinite rl
    |> Stream.takeWhile (fun _ -> eof() |> not)
  
let writeLinesS (filePath:string) (lines: Stream<string>) = 
    use sr = new System.IO.StreamWriter (filePath, false)
    lines |> Stream.iter sr.WriteLine
    sr.Close()

let setLine2csvLine (setLine: string)  = 
    Seq.fold (fun acc v -> acc + "," + (string v)) (string setLine.[0]) (Seq.skip 1 setLine)    
   
let set2csv setFilePath csvFilePath =     
    readLines setFilePath
    |> Seq.map setLine2csvLine
    |> writeLines csvFilePath

// raed csv, all columns contains decimal numbers
let readCSV (filePath:string) isHeader (inputCols: int[]) (outputCol: int)= 
    let mapLine (str: string) = 
        let cols = str.Split([|','|])
        let outs = seq { for i in inputCols -> System.Double.Parse(string cols.[i]) } |> Seq.toArray
        outs, System.Double.Parse(cols.[outputCol])
        
    readLinesS filePath
    |> Stream.skip (if isHeader then 1 else 0)
    |> Stream.map mapLine
    |> Stream.toArray
    |> Array.unzip

