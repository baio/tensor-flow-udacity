module DataProccessing.Utils

open Types

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

