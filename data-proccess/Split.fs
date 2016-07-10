module DataProccessing.Split

open Utils
open Measures
open Shuffle

type TVTSamples<'a> = { train: 'a; validation: 'a; test: 'a }
type TVTSampleSizes = TVTSamples<int<percent>>
type TVTSamplePaths = TVTSamples<string>

// Shuffle and split data set
let splitTrainValidTest (linesNumber: int) (filePath: string) (sizes: TVTSampleSizes) (paths: TVTSamplePaths) =

    if (sizes.test + sizes.train + sizes.validation > 100<percent>) then
        failwith("Sample sizes in sum must be less then 100%")

    let getSamplingCount (size: int<percent>) =
        float linesNumber * (float size / 100.)  |> System.Math.Floor |> int

    let lines = 
        readLines filePath 
        |> shuffle (new System.Random()) linesNumber
    
    let trainCount = getSamplingCount sizes.train
    let validCount = getSamplingCount sizes.validation
    let testCount = getSamplingCount sizes.validation

    lines 
    |> Seq.take trainCount
    |> writeLines paths.train
    
    lines 
    |> Seq.skip trainCount
    |> Seq.take validCount
    |> writeLines paths.validation

    lines 
    |> Seq.skip (trainCount + validCount)
    |> Seq.take testCount
    |> writeLines paths.test
    
