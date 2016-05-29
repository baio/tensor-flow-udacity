module a_1_p_5.spec

open NUnit.Framework
open utils
open FsUnit

open a_1_p_5

[<Test>]
let ``Calc overlaps`` () =

    let tvt = (
        [("A", 1); ("A", 2); ("B", 3)], 
        [("A", 1); ("A", 5); ("B", 3)], 
        [("C", 1); ("C", 2); ("C", 3)]
    )
        
    let overlaps = calcOverlaps tvt

    overlaps |> should equal (2, 0)


