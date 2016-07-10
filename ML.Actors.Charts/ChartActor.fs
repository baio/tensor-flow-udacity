module ML.Actors.Charts.ChartActor

open Akka.FSharp
open Akka.FSharp.Spawn
open Akka.Actor

open DataProccessing.ReadImages
open ML.Charts.R.Image
open Nessos.Streams

type ChartType = R

type ChartOpts = {ChartType : ChartType; DataPath : string; DataCount : int }
type ChartStoreOpts = { Opts : ChartOpts; OutputExt: string; OutputPath : string;  }

type ChartMessgae = 
    | ChartShow of ChartOpts
    | ChartStore of ChartStoreOpts

let ChartActor (mailbox:Actor<ChartMessgae>) =
           
    let rec chart () = 
        actor {         
            let! message = mailbox.Receive();
            match message with 
            | ChartShow opts ->
                getImagesBytes opts.DataPath
                |> Stream.take opts.DataCount
                |> Stream.toArray
                |> showImagesBW
                return! chart()
            | ChartStore opts ->
                failwith("Not implemented")                                 
        }

    chart()
