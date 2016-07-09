module ShowImages

//open image
open System.IO
open DataProccessing.ReadImages


let showImages count path   =
    getImagesBytes path
    |> Seq.take count
    |> Seq.toArray
    //|> showImagesBW
    