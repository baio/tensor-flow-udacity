module a1_p_2_test

open a_1_p_2

open Fuchu

//[<Tests>]
let a_1_p_2_Test_1 = 
    
    testCase "Get pixels from bytes array (4*4*4)" <| 
        fun _ ->
            let sq = Seq.init 24 (fun f -> 1)        
            let actual = getPixels sq |> Seq.toArray
            let expected = [|(single)1.401298E-45; (single)1.401298E-45|]

            Assert.Equal("get pixels", expected, actual)

//[<Tests>]
(*
let a_1_p_2_Test_2 = 
    
    testCase "Read file and show image" <| 
        fun _ ->
            let path = "../../../data/out/letters-bl/A.bl"
            readFileAndShowImage path 
*)

