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

let readPermutesFromBinaries() =     
    let permute = (getDataPath "out/letters-bl/A.bl"),  ([0; 1; 2], [0; 3], [5; 6])
    let bytes = readPermute (imagePixel.ConvertToByte IMAGE_LENGTH) permute 
    printf "%A" bytes
    System.Console.ReadKey() |> ignore

let permutesRead() =     
    let random = new System.Random(1)
    let rnd max = random.Next max
    let result = permutesAndRead (getDataPath "out/letters-bl") (imagePixel.ConvertToByte IMAGE_LENGTH) rnd 10 5 5
    printf "%A" result
    System.Console.ReadKey() |> ignore


let permutesShow showImages =     
    let random = new System.Random()
    let rnd max = random.Next max
    let result = permutesAndRead (getDataPath "out/letters-bl") (imagePixel.ConvertToByte IMAGE_LENGTH) rnd 15 15 10
    let train, test, valid = snd result.Head
    showImages (train |> List.toArray |> Array.map (fun m -> m |> getPixels |> Seq.toArray)) 
    System.Console.ReadKey() |> ignore
