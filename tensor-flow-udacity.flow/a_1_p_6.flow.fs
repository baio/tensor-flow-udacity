module a_1_p_6.flow

open utils
open config
open measures

open a_1_p_6


let classify() = 
    
    let data = readData (getDataPath "out/tvt/train.d")
    let labels = readLabels (getDataPath "out/tvt/trainl.d")

    let seq2array (s : seq<_>) = s |> Seq.map float |> Seq.toArray

    let x = data |> Seq.map seq2array |> Seq.toArray 
    let y = (lettersToScores (Seq.toArray labels)) |> Array.map float

    let res = calcLinear x y

    printf "%A" res

    System.Console.ReadKey() |> ignore

    
    