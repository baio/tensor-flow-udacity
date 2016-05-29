module a_1_p_5.flow

open utils
open config
open measures
open a_1_p_4
open a_1_p_5

//http://stackoverflow.com/questions/13610074/is-there-a-rule-of-thumb-for-how-to-divide-a-dataset-into-training-and-validatio

let showOverlap() = 
    let gp name = getDataPath (sprintf "out/tvt/%s.d" name)

    let paths = {
        train = (gp "train")
        test = (gp "test")
        validate = (gp "valid")
        trainLabel = (gp "trainl")
        testLabel = (gp "testl")
        validateLabel = (gp "validl")
        trainIndex = (gp "traini")
        testIndex = (gp "testi")
        validateIndex = (gp "validi")
    }

    let sets = readSetsLabelIndex paths
    let overlaps = calcOverlaps sets

    let tr, _, _ = sets
    printfn "Total %A" tr.Length
    printfn "%A" overlaps

    System.Console.ReadKey() |> ignore