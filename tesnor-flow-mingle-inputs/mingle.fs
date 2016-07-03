module mingle

open utils
open System.IO

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let writeLines (filePath:string) (lines: string seq) = 
    use streamWriter = new System.IO.StreamWriter(new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
    lines |> Seq.iter streamWriter.WriteLine 
    streamWriter.Close()

let readLinesCount (filePath: string) = 
    readLines filePath
    |> Seq.fold (fun acc _ -> acc + 1) 0    
    
let mingleSamples inFile outFile =
        
    // TODO : read in batches
    // Need to know number of lines in advance
    // Outpuf file size different from input ?!
                
    let lines = readLines inFile |> Seq.toArray
    let cnt = lines.Length
       
    generateShuffled cnt
    |> Array.map (fun i -> lines.[i])   
    |> writeLines outFile
        
    cnt     

[<Measure>] 
type percent

type TVTSamples<'a> = { train: 'a; validation: 'a; test: 'a }
type TVTSampleSizes = TVTSamples<int<percent>>
type TVTSamplePaths = TVTSamples<string>
    
let splitTrainValidTest (cnt: int) (mingledFile: string) (sizes: TVTSampleSizes) (paths: TVTSamplePaths) =

    if (sizes.test + sizes.train + sizes.validation > 100<percent>) then
        failwith("Sample sizes in sum must be less then 100%")

    let getSamplingCount (size: int<percent>) =
        float cnt * (float size / 100.)  |> System.Math.Floor |> int

    let lines = readLines mingledFile
    
    let trainCount = getSamplingCount sizes.train
    lines 
    |> Seq.take trainCount
    |> writeLines paths.train

    let validationCount = getSamplingCount sizes.validation
    lines 
    |> Seq.skip trainCount
    |> Seq.take validationCount
    |> writeLines paths.validation

    let testCount = getSamplingCount sizes.test
    lines 
    |> Seq.skip (trainCount + validationCount)
    |> Seq.take testCount
    |> writeLines paths.test
