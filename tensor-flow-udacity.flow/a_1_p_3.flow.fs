module a_1_p_3.flow

open config

open measures
open utils
open a_1_p_3

let readLetterNumbersFromBinaries() = 
    let number = readNumberOfLetters (getDataPath "out/letters-bl")  (imagePixel.ConvertToByte IMAGE_LENGTH)
    printf "Number of letters %A" number
    System.Console.ReadKey() |> ignore


let readLetterNumbersFromBinaries1() = 
    let number = readNumberOfLetters (getDataPath "out/letters-bl")  (imagePixel.ConvertToByte IMAGE_LENGTH)
    printf "Number of letters %A" number
    System.Console.ReadKey() |> ignore
