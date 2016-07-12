module ML.Math.RunModel

open DataProccessing.Utils
open ML.Math.Types

// file contains
// samples - rows
// first char - label, tail chars - features
let runModelFromFile (numberOfClasses : int) runModel filePath  = 
    readLinesS filePath
    |> Samples2Tulpes
    |> runModel numberOfClasses