module utils

open Accord.IO

type InputItem =  float[] * int 
       
let readCSV path (mapLine : string array -> InputItem) = 
    use file = new System.IO.StreamReader(new System.IO.FileStream(path, System.IO.FileMode.Open))
    let csv = new CsvReader(file, true)
    let lines = csv.ReadToEnd();
    lines |> List.ofSeq |> List.map mapLine



