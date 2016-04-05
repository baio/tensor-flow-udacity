module a1_p_1_test

open utils
open a_1_p_1

open Fuchu


let list2rgb(s : _ array) =
    s.[0], s.[1], s.[2]


[<Tests>]
let a_1_p_1_Test_1 = 
    testCase "Read letter and check image size" <| 
        fun _ -> 
            let path = "../../../data/letters/A/a.png"
            let pixels = path |> loadBitmap 

            Assert.Equal("Lenght 1 should be 28", 28, pixels.GetLength 0)
            Assert.Equal("Lenght 2 should be 28", 28, pixels.GetLength 1)

[<Tests>]
let a_1_p_1_Test_2 = 
    testCase "Read letter and check flatted array size" <| 
        fun _ -> 
            let path = "../../../data/letters/A/a.png"
            let pixels = path |> loadBitmap |> flatSingleImagePixels

            Assert.Equal("Size of flatted array incorrect", 28 * 28 * 3, pixels.Length)

[<Tests>]
let a_1_p_1_Test_3 = 
    testCase "Read letter and check greyscaled pixels length" <| 
        fun _ -> 
            let path = "../../../data/letters/A/a.png"
            let pixels = path |> loadBitmap |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(fun m -> m |> list2rgb |> getGrayScale) |> Seq.toArray

            Assert.Equal("Size of flatted array incorrect", 28 * 28, pixels.Length)

[<Tests>]
let a_1_p_1_Test_4 = 
    testCase "Read letter and show un / normalized image" <| 
        fun _ -> 
            setPar [|1;2|]
            let path = "../../../data/letters/A/a.png"
            
            let pixels1 = path |> loadBitmapNormalized |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(fun m -> m |> list2rgb |> getGrayScale) |> Seq.toArray
            pixels1 |> Array.map (fun m -> (float)m) |> showImage |> ignore

            let pixels2 = path |> loadBitmap |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(fun m -> m |> list2rgb |> getGrayScale) |> Seq.toArray
            pixels2 |> Array.map (fun m -> (float)m) |> showImage |> ignore
