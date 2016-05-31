module a_1_p_6.spec

open NUnit.Framework
open utils
open FsUnit

open a_1_p_6

[<Test>]
let ``Softmax 1`` () =

    let scores = [|[|3.; 1.; 0.2|]|]
        
    let actual = softmax scores
    let expected = [|[|0.8360188027814407; 0.11314284146556014; 0.050838355752999165|]|]

    actual |> should equal (expected)


[<Test>]
let ``Softmax 2`` () =

    let scores = [|[|1.; 2.; 3.; |];
                   [|2.; 4.; 8.; |];
                   [|3.; 5.; 7.; |];
                   [|6.; 6.; 6.; |];
                 |]
        
    let actual = softmax scores
    let expected = [|
        [|0.090030573170380462; 0.24472847105479764; 0.66524095577482178|];
        [|0.002428258029591338; 0.017942534803329198; 0.97962920716707946;|];
        [|0.015876239976466769; 0.11731042782619837; 0.86681333219733481;|];
        [|0.33333333333333331; 0.33333333333333331; 0.33333333333333331|]
    |]

    actual |> should equal (expected)


