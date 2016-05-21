module a_1_p_3

open measures
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

let readNumberOfLetters dirName (imageLength: int<imageByte>) = 
    let fis = new System.IO.DirectoryInfo(dirName)    
    let getLettersNumber fileLength = (int)(fileLength / (int64)imageLength)
    
    fis.GetFiles() 
    |> Array.map (fun fi -> fi.Name.Replace(fi.Extension, ""), (getLettersNumber fi.Length))

let letterPermutes (numberOfLetters : (string * int) list) rnd trainSize testSize validSize =     
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
    


