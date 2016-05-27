// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

module flow.main

let SELECT_OPTIONS = "
    1. Show letter
    2. Show normalized letter
    3. Create letters binaries
    4. Show letter images from binaries
    5. Read letter numbers from binaries
    6. Permutes - Read bytes from binaries
"

open a_1_p_1.flow
open a_1_p_2.flow
open a_1_p_3.flow

let showImages = utils.showImages tensor_flow_udacity.r.utils.setPar tensor_flow_udacity.r.utils.showImage 


let runOption (option : System.ConsoleKeyInfo) = 
    match option.KeyChar with
        | '1' -> showLetter showImages
        | '2' -> showNormalizedLetter showImages
        | '3' -> createLetterBinaries()
        | '4' -> showLetterImagesFromBinaries showImages
        | '5' -> readLetterNumbersFromBinaries()
        | '6' -> readPermutesFromBinaries()
        | _ -> printfn "X \\"


let rec chooseOption _ =
    printfn "%s" SELECT_OPTIONS
    System.Console.ReadKey() |> runOption |> ignore
    //chooseOption()
    
[<EntryPoint>]
let main argv = 
    chooseOption()
    0 // return an integer exit code
