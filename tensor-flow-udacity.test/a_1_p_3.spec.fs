module a_1_p_3.spec

open NUnit.Framework
open utils
open FsUnit

open a_1_p_3

[<Test>]
let ``Render 5 items with randomized range [1..10]`` () =
        let random = new System.Random(1)
        let rnd max = random.Next max
        let permutes = rpermute rnd 10 5 []

        printf "%A" permutes

        permutes |> should equal [6; 8; 5; 0; 2]


[<Test>]
let ``Render train, test, valid sets with 2 elements, randomized ranges 6, 5, 4`` () =

        let random = new System.Random(1)
        let limits = [6; 5; 4]
        let rnd max = random.Next max
        let permutes = permute rnd limits 2 2 2

        //main pint here : validation set (3d) shouldn't contain elements from training set (1st)
        //value for each element shouldn't exceed limit from limits
        permutes |> should equal ([[0; 1]; [4; 2]; [1; 2]], [[5; 2]; [3; 0]; [1; 0]], [[5; 3]; [1; 3]; [3; 0]])


[<Test>]
let ``Render train, test, by letters number`` () =

        let random = new System.Random(1)
        let rnd max = random.Next max
        let numberOfLetters = [("a", 6); ("b", 7)]
        
        let permutes = letterPermutes rnd 2 2 1 numberOfLetters

        permutes |> should equal [ ("a", ( [0; 1], [2; 3], [2]) ); ("b", ( [5; 3], [6; 2], [4]) ) ]

