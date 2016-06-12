(*
let takeEveryNth n lst = 
    lst |> List.mapi (fun i el -> el, i)              // Add index to element
        |> List.filter (fun (el, i) -> i % n = 0) // Take every nth element
        |> List.map fst                               // Drop index from the result
*)

#r @"C:\dev\tensor-flow-udacity\packages\MathNet.Numerics.3.11.1\lib\net40\MathNet.Numerics.dll"

open MathNet.Numerics

let a, b = Fit.Line ([|10.0;20.0;30.0|], [|15.0;20.0;25.0|])

let x = [|
    [|1.; 4.|];
    [|2.; 5.|];
    [|3.; 2.|];
|]

let y = [|
    15.; 20.; 10.
|]

let p = Fit.MultiDim(x, y, true)

let func (z: float array) =     
    printfn "%A" z
    z.[0]

let p1 = Fit.LinearMultiDim(x, y, func)

//Fit.LinearGenericFunc()