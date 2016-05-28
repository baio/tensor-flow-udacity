module a_1_p_4
(*
Problem 4
Convince yourself that the data is still good after shuffling!
Finally, let's save the data for later reuse:
*)

open types

type TTVPaths = {
    train : string 
    test : string 
    validate : string 
    trainLabel : string 
    testLabel : string 
    validateLabel : string 
    }

type TTVFiles = {
    trainFile: System.IO.StreamWriter
    testFile: System.IO.StreamWriter
    validFile: System.IO.StreamWriter
    trainLabelFile: System.IO.StreamWriter
    testLabelFile: System.IO.StreamWriter
    validLabelFile: System.IO.StreamWriter
}


let storeSet (files: TTVFiles) (prm : TTVPermutes<byte array>)=
    let listFlatten lsit = lsit |> List.toArray |> Array.collect (fun f -> f)
    
    let letter, ttvs = prm
    let train, test, valid = ttvs
    
    let trainFlatten = listFlatten train
    let testFlatten = listFlatten test
    let validFlatten = listFlatten valid
    
    let trainLabels = Array.create train.Length, char
    let testLabels = Array.create train.Length, char
    let validLabels = Array.create train.Length, char

    files.trainFile.Write(trainFlatten) |> ignore
    files.testFile.Write(testFlatten) |> ignore
    files.validFile.Write(validFlatten) |> ignore
    
    files.trainLabelFile.Write(trainLabels) |> ignore
    files.testLabelFile.Write(testLabels) |> ignore
    files.validLabelFile.Write(validLabels) |> ignore
        
let storeTTV (paths : TTVPaths) (prms : TTVPermutes<byte array> list)=
    
    let createFile path = new System.IO.StreamWriter(new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Write))
    
    let files = {
        trainFile = createFile paths.train
        testFile = createFile paths.train
        validFile = createFile paths.train
        trainLabelFile = createFile paths.trainLabel
        testLabelFile = createFile paths.testLabel
        validLabelFile = createFile paths.validateLabel
    }        
            
    prms |> List.iter (storeSet files)
    