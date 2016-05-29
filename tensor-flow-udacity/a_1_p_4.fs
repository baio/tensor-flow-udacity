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
    trainIndex : string 
    testIndex : string 
    validateIndex : string 
    }

type TTVFiles = {
    trainFile: System.IO.StreamWriter
    testFile: System.IO.StreamWriter
    validFile: System.IO.StreamWriter
    trainLabelFile: System.IO.StreamWriter
    testLabelFile: System.IO.StreamWriter
    validLabelFile: System.IO.StreamWriter
    trainIndexFile: System.IO.StreamWriter
    testIndexFile: System.IO.StreamWriter
    validIndexFile: System.IO.StreamWriter
}

let write (stream: System.IO.StreamWriter) (bytes: 'a[]) = bytes |> Array.iter stream.Write


let storeSet (files: TTVFiles) (prm : TTVPermutes<int * byte array>)=
    let listFlatten lsit = lsit |> List.toArray |> Array.collect (fun f -> f)
    
    let letter, ttvs = prm
    let train, test, valid = ttvs
    
    let trainFlatten = listFlatten (train |> List.map snd)
    let testFlatten = listFlatten (test |> List.map snd)
    let validFlatten = listFlatten (valid |> List.map snd)

    let trainIx = train |> List.map fst |> List.toArray |> Array.map (sprintf "%i;")
    let testIx = test |> List.map fst |> List.toArray |> Array.map (sprintf "%i;")
    let validIx = valid |> List.map fst |> List.toArray |> Array.map (sprintf "%i;")
    
    let trainLabels = Array.create train.Length letter
    let testLabels = Array.create test.Length letter
    let validLabels = Array.create valid.Length letter

    write files.trainFile trainFlatten
    write files.testFile testFlatten
    write files.validFile validFlatten
    
    write files.testLabelFile testLabels
    write files.trainLabelFile trainLabels
    write files.validLabelFile validLabels

    write files.trainIndexFile trainIx
    write files.testIndexFile testIx
    write files.validIndexFile validIx
        
let storeTTV (paths : TTVPaths) (prms : TTVPermutes<int * byte array> list)=
    
    let createFile path = new System.IO.StreamWriter(new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
    
    let files = {
        trainFile = createFile paths.train
        testFile = createFile paths.test
        validFile = createFile paths.validate
        trainLabelFile = createFile paths.trainLabel
        testLabelFile = createFile paths.testLabel
        validLabelFile = createFile paths.validateLabel
        trainIndexFile = createFile paths.trainIndex
        testIndexFile = createFile paths.testIndex
        validIndexFile = createFile paths.validateIndex
    }        
            
    prms |> List.iter (storeSet files)

    files.testFile.Close()
    files.trainFile.Close()
    files.validFile.Close()
    files.trainLabelFile.Close()
    files.testLabelFile.Close()
    files.validLabelFile.Close()
    files.trainIndexFile.Close()
    files.testIndexFile.Close()
    files.validIndexFile.Close()


    