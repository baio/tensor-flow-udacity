// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

module flow.main

open a_1_p_6

let SELECT_OPTIONS = "
    1. Show letter
    2. Show normalized letter
    3. Create letters binaries
    4. Show letter images from binaries
    5. Read letter numbers from binaries
    6. Permutes - Read bytes from binaries
    7. Permutes and read
    8. Permutes and show
    9. Store permutes
    0. Show permutes overlap
"

open a_1_p_1.flow
open a_1_p_2.flow
open a_1_p_3.flow
open a_1_p_4.flow
open a_1_p_5.flow

let showImages = utils.showImages tensor_flow_udacity.r.utils.setPar tensor_flow_udacity.r.utils.showImage 


let runOption (option : System.ConsoleKeyInfo) = 
    match option.KeyChar with
        | '1' -> showLetter showImages
        | '2' -> showNormalizedLetter showImages
        | '3' -> createLetterBinaries()
        | '4' -> showLetterImagesFromBinaries showImages
        | '5' -> readLetterNumbersFromBinaries()
        | '6' -> readPermutesFromBinaries()
        | '7' -> permutesRead()
        | '8' -> permutesShow showImages
        | '9' -> storePermutes()
        | '0' -> showOverlap()
        | _ -> printfn "X \\"


let rec chooseOption _ =
    printfn "%s" SELECT_OPTIONS
    System.Console.ReadKey() |> runOption |> ignore
    //chooseOption()
    
[<EntryPoint>]
let main argv = 
    //storePermutes()
    chooseOption()
    //calcLinear()
    0 // return an integer exit code
