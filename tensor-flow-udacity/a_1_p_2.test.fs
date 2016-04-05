module a1_p_2_test

open a_1_p_2

open Fuchu

[<Tests>]
let a_1_p_2_Test_2 = 
    
    testCase "Read file and show image" <| 
        fun _ ->
            let path = "../../../data/out/letters-bl/A.bl"
            readFileAndShowImage path 
