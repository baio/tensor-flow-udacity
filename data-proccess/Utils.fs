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
