module ML.Math.Types
open Nessos.Streams

//type Sample<'l, 'f> = { Label : 'l; Features : 'f array}
//type Samples<'l, 'f> = Sample<'l, 'f> array

//each sample contains - 1st element label, 2nd - array of features
type Sample<'a> = 'a array
//array of samples
type Samples<'a> = 'a Stream

//Given samples as string of bytes, return tulpe of labels (int) and features (float)
let Samples2Tulpes (samples: Stream<string>) : int [] * float [][] = 
    samples 
    |> Stream.map (Seq.toArray >> Seq.map (fun m -> m |> string |> System.Int32.Parse) >> Seq.toArray)
    |> Stream.map (fun m -> int (m.[0] + 1), (m.[1..] |> Array.map float))
    |> Stream.take 50
    |> Stream.toArray
    |> Array.unzip
    