module a_1_p_3.flow

open config

open utils
open a_1_p_3

let readLetterNumbersFromBinaries() = 
    let number = readNumberOfLetters (getDataPath "out/letters-bl")  (IMAGE_LENGTH * 3 * 4)
    printf "Number of letters %A" number
    System.Console.ReadKey() |> ignore
