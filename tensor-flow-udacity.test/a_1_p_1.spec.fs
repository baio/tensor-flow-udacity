module tensor_flow_udacity.test1

open NUnit.Framework
open FsUnit

open utils
open a_1_p_1

let LETTERS_PATH = "C:/dev/tensor-flow-udacity/data/letters/"

let getLetterPath letter = LETTERS_PATH + letter + ".png"

[<Test>]
let ``Read 'A' letter, both dimentions should be 28 pixels`` () =
        let path = getLetterPath "A/a"
        let pixels = path |> loadBitmap 

        pixels.GetLength 0 |> should equal 28
        pixels.GetLength 1 |> should equal 28

[<Test>]
let ``Read 'A' letter, flatenned array size should be 28 * 28 * 3, since each dimention is 28 and 3 bytes for R G B of each pixel`` () =
        let path = getLetterPath "A/a"
        let pixels = path |> loadBitmap |> flatSingleImagePixels

        pixels.Length |> should equal (28 * 28 * 3)

[<Test>]
let ``Read 'A' letter, grayscaled pixels length should be 28 * 28,  since each dimention is 28 and greyscaled color just one byte`` () =
        let path = getLetterPath "A/a"
        let RGB2GrayScale (arr : byte array) = getGrayScale (arr.[0], arr.[1], arr.[2])
        let pixels = path |> loadBitmap |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(RGB2GrayScale) |> Seq.toArray

        pixels.Length |> should equal (28 * 28)
