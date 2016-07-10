module ML.Actors.SplitSetActor

open Akka.Actor
open Akka.FSharp

open DataProccessing.Split

type TVTSplitOutput = {Sizes : TVTSampleSizes; Paths : TVTSamplePaths}
type TVTSplit = { InputFilePath : string; InputSamplesCount: int; Output: TVTSplitOutput}

type TVTSplitSetCommands =
    | TVTSplitSet of TVTSplit
    | TVTSplitSetComplete

let SplitSetActor (mailbox: Actor<TVTSplitSetCommands>) = 
               
    let rec loop() = 
        actor {
    
            let! msg = mailbox.Receive()

            match msg with 
            | TVTSplitSet opts -> 
                splitTrainValidTest opts.InputSamplesCount opts.InputFilePath opts.Output.Sizes opts.Output.Paths            
                mailbox.Sender() <! TVTSplitSetComplete
            | _ -> ()

            return! loop()
        }

    loop()

