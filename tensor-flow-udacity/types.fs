module types

type TTVSets<'a>  = ('a list) * ('a list) * ('a list)
type TTVPermutes<'a> = string * TTVSets<'a>


type Vector = byte[]

type SetSample = { label: string; index: int; data : Vector }

type SetSampleTyped = 
    | TrainSample of SetSample
    | ValidSample of SetSample
    | TestSample of SetSample

