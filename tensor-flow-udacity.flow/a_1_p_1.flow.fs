module a_1_p_1.flow

open config

open utils
open a_1_p_1

let showLetter showImages = 
    let path = getLetterPath "A/a"
            
    let pixels = 
        path |>
        loadBitmap |> 
        flatSingleImagePixels |> 
        Seq.chunkBySize 3 |> 
        Seq.map(RGB2GrayScale) |> 
        Seq.toArray
                
    [|pixels|] |> showImages 


let showNormalizedLetter showImages = 
    let path = getLetterPath "A/a"
            
    let pixels =
        path |> 
        loadBitmapNormalized |> 
        Option.get |> 
        flatSingleImagePixels |> 
        Seq.chunkBySize 3 |> 
        Seq.map(RGB2GrayScale) |> 
        Seq.toArray            
                
    [|pixels|] |> showImages 

