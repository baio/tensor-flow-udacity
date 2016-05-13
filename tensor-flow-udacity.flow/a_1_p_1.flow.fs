module a_1_p_1.flow

open config

open utils
open a_1_p_1

let showLetter showImages = 
    let path = getLetterPath "A/a"
            
    let pixels = path |> loadBitmap |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(RGB2GrayScale) |> Seq.toArray
                
    [|pixels|] |> showImages 


let showNormalizedLetter showImages = 
    let path = getLetterPath "A/a"
            
    let pixels = path |> loadBitmapNormalized |> Option.get |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(RGB2GrayScale) |> Seq.toArray            
                
    [|pixels|] |> showImages 


(*
let a_1_p_1_Test_4 = 
    testCase "Read letter and show un / normalized image" <| 
        fun _ -> 

            let path = "../../../data/letters/A/a.png"
            
            let pixels1 = path |> loadBitmapNormalized |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(fun m -> m |> list2rgb |> getGrayScale) |> Seq.toArray            
            let pixels2 = path |> loadBitmap |> flatSingleImagePixels |> Seq.chunkBySize 3 |> Seq.map(fun m -> m |> list2rgb |> getGrayScale) |> Seq.toArray
            
            [|pixels1; pixels2|] |> showImages 

*)

