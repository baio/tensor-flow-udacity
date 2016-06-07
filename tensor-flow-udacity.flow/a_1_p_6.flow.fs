module a_1_p_6.flow

open utils
open config
open measures

open a_1_p_3
open a_1_p_4
open a_1_p_6

let classify() = 

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
    
    let samples = readSetSamples paths 10

    printf "%A" samples

    System.Console.ReadKey() |> ignore

    
    