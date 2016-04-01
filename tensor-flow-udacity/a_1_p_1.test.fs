module a1_p_1_test

open a_1_p_1

open Fuchu

[<Tests>]
let simpleTest = 
    testCase "A simple test" <| 
        fun _ -> Assert.Equal("2+2", 4, 2+2)