module DataProccessing.Split

open Utils
open Measures

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
