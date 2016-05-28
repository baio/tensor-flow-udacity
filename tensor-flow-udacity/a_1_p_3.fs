module a_1_p_3

open utils
open measures
open types

(*
Problem 3
Another check: we expect the data to be balanced across classes. Verify that.
Merge and prune the training data as needed. Depending on your computer setup, you might not be able to fit it all in memory, and you can tune train_size as needed. The labels will be stored into a separate array of integers 0 through 9.
Also create a validation dataset for hyperparameter tuning.
*)


///Generate array of items of length num with random elements from range 0..max
///Exclued `exclude` elements from generated array
let rpermute rnd max num (exclude : _ list) =
    let mutable seen = []
    while seen.Length < num do
        let r = rnd max
        if not (List.contains r seen) && not (List.contains r exclude) then
            seen <- r::seen
    seen            
        
///trianSize testSize validSize - numbers of each letter in sets
let permute rnd (limits : int list) trainSize testSize validSize  =     
    let prm size excl mx  = rpermute rnd mx size excl
    let train = limits |> List.map (prm trainSize [])
    let test = limits |> List.map (prm testSize [])
    let valid = limits |> List.mapi (fun i e -> prm validSize train.[i] e)

    train, test, valid

let takeEveryNth n lst = 
  lst |> List.mapi (fun i el -> el, i)              // Add index to element
      |> List.filter (fun (el, i) -> i % n = 0) // Take every nth element
      |> List.map fst                               // Drop index from the result

/// Read number of samples for each letter
let readNumberOfLetters dirName (imageLength: int<imageByte>) = 
    let fis = new System.IO.DirectoryInfo(dirName)    
    let getLettersNumber fileLength = (int)(fileLength / (int64)imageLength)
    
    fis.GetFiles() 
    |> Array.map (fun fi -> fileJustName <| fi , (getLettersNumber fi.Length))

/// Inputs :
/// numberOfLetters - List of pairs, letter * max number of items in set
/// rnd - random generator  
/// trainSize - number of items to generate in train set (shouldnt exceed max number of items in set)
/// ...
/// Output :
/// List of tulpes - letter * (tulpe with 3 elements train set (list of gen items), test ..., valid ...)
let letterPermutes rnd trainSize testSize validSize (numberOfLetters : (string * int) list) =     
    let limits = numberOfLetters |> List.map snd
    let t, tt, v = permute rnd limits trainSize testSize validSize
    
    // t:[a; b;], tt:[a; b;], v:[a; b;] -> 
    // [t:a, t:b, tt:a, tt:b, v:a, v:b] ->
    // [[t:a; tt:a, v:b]; [t:b; tt:b; v:b]] ->
    // [a(t, tt, v); b(t, tt, v)]

    // t:[a; b;], tt:[a; b;], v:[a; b;]
    let l1 = List.concat([t; tt; v]) 
    //[t:a, t:b, tt:a, tt:b, v:a, v:b]    
    let l2 = List.init t.Length (fun i -> takeEveryNth t.Length l1.[i..])
    // [[t:a; tt:a, v:a]; [t:b; tt:b; v:b]]

    let setsList2Tulpe (lst: 'a list) = lst.[0], lst.[1], lst.[2]
    
    numberOfLetters 
    |> List.zip l2
    |> List.map(fun (prs, letter) -> fst letter, (setsList2Tulpe prs))

let readFilePosition (file: System.IO.FileStream) (fromPos: int) (length: int) = 
    file.Seek((int64 fromPos * int64 length), System.IO.SeekOrigin.Begin) |> ignore
    let b = Array.create length (byte 0)
    file.Read(b, 0, length) |> ignore
    b

let readFilePositions (file: System.IO.FileStream) fromPoses length =
    fromPoses
    |> List.map (fun pos -> readFilePosition file pos length)

/// Inputs :
/// permute string * TTVSet
/// string - path to file
/// TTVSet - positions to read from file for each set train, test, validation
/// length - length of bytes to read starting from position
/// Outputs : 
/// tulpe with train, test, valid
/// array of read bytes from position and of length `length`
let readPermute (length : int<imageByte>) (permute: TTVPermutes<int>) =
    let path, sets = permute
    let train, test, valid = sets
    use file = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read)
    let trainBytes = readFilePositions file train (int length)
    let testBytes = readFilePositions file test (int length)
    let validBytes = readFilePositions file valid (int length)
    trainBytes, testBytes, validBytes

let readPermutes length (permutes: TTVPermutes<int> list) =
    permutes |> List.map (fun p -> readPermute length p)

/// Create permutes for each set in directory and then read them
/// Output : (Letter, (train: array list, test: ..., valid: ...)))
/// Letter - name of the file in directory
/// tulpe of sets - traint, test, valid
/// each tulpe contains list of images
/// images is array of bytes
let permutesAndRead dirName (imageLength: int<imageByte>) rnd trainSize testSize validSize =
    let perm = letterPermutes rnd trainSize testSize validSize
    let permutes = perm <| (readNumberOfLetters dirName imageLength |> Array.toList)
    let getLetterFileName letter = sprintf "%s/%s.bl" dirName letter
    let readPermute = readPermute imageLength
    let getPermute letter set = (getLetterFileName letter), set
        
    permutes
    |> List.map (fun (letter, set) -> letter, (readPermute (getPermute letter set)))

    
    



