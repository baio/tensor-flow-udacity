module ML.Actors.Types

open DataProccessing.Types

type RWMessage = 
    | RWStart of InputOutputPaths
    | RWFileComplete
    | RWClosed of int * string

    

