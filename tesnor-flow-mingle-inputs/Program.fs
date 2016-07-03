// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open mingle

[<EntryPoint>]
let main argv = 
    
    //let cnt = mingleSamples @"C:\dev\.data\notMNIST_normalized.csv" @"C:\dev\.data\notMNIST_normalized_mingled.csv"
   
    let tvtSizes = {
        train = 70<percent>; 
        validation = 15<percent>; 
        test = 15<percent>
    }
    
    let tvtPaths = { 
        train = @"C:\dev\.data\notMNIST_train.csv"; 
        validation = @"C:\dev\.data\notMNIST_valid.csv"; 
        test = @"C:\dev\.data\notMNIST_test.csv"; 
    }

    splitTrainValidTest 18724 @"C:\dev\.data\notMNIST_normalized_mingled.csv" tvtSizes tvtPaths

    printfn "%A" argv 
    //18724
    0 // return an integer exit code
