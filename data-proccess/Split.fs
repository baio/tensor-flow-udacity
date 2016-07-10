module DataProccessing.Split

open Utils
open Measures
open Shuffle

open Nessos.Streams

type TVTSamples<'a> = { train: 'a; validation: 'a; test: 'a }
type TVTSampleSizes = TVTSamples<int<percent>>
type TVTSamplePaths = TVTSamples<string>

// Shuffle and split data set
let splitTrainValidTest (linesNumber: int) (filePath: string) (sizes: TVTSampleSizes) (paths: TVTSamplePaths) =

    let stopWatch = System.Diagnostics.Stopwatch.StartNew()

    if (sizes.test + sizes.train + sizes.validation > 100<percent>) then
        failwith("Sample sizes in sum must be less then 100%")

    let getSamplingCount (size: int<percent>) =
        float linesNumber * (float size / 100.)  |> System.Math.Floor |> int

    let lines = 
        readLines filePath 
        |> Array.ofSeq
        |> shuffleS (new System.Random()) linesNumber
    
    let trainCount = getSamplingCount sizes.train
    let validCount = getSamplingCount sizes.validation
    let testCount = getSamplingCount sizes.validation

    lines 
    |> Stream.take trainCount
    |> writeLinesS paths.train
    
    lines 
    |> Stream.skip trainCount
    |> Stream.take validCount
    |> writeLinesS paths.validation

    lines 
    |> Stream.skip (trainCount + validCount)
    |> Stream.take testCount
    |> writeLinesS paths.test
    
    stopWatch.Stop()
    printfn "%f" stopWatch.Elapsed.TotalMilliseconds
