module a_1_p_4.flow

open utils
open config
open measures
open a_1_p_3
open a_1_p_4

//http://stackoverflow.com/questions/13610074/is-there-a-rule-of-thumb-for-how-to-divide-a-dataset-into-training-and-validatio

let storePermutes() =     
    //18720
    // 30 examples 
    
    //1496 374 374
    let random = new System.Random()
    let rnd max = random.Next max
    let gp name = getDataPath (sprintf "out/tvt/%s.d" name)
    let paths = {
        train = (gp "train")
        test = (gp "test")
        validate = (gp "valid")
        trainLabel = (gp "trainl")
        testLabel = (gp "testl")
        validateLabel = (gp "validl")
    }

    //generate

    let prm = permutesAndRead (getDataPath "out/letters-bl") (imagePixel.ConvertToByte IMAGE_LENGTH) rnd 1496 374 374
    let result = storeTTV paths prm
    System.Console.ReadKey() |> ignore
