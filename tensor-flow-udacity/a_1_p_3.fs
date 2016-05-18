module a_1_p_3

(*
Problem 3
Another check: we expect the data to be balanced across classes. Verify that.
Merge and prune the training data as needed. Depending on your computer setup, you might not be able to fit it all in memory, and you can tune train_size as needed. The labels will be stored into a separate array of integers 0 through 9.
Also create a validation dataset for hyperparameter tuning.
*)


///Generate array of items of length num with random elements from range 0..max
///Exclued `exclude` elements from generated array
let rpermute rnd max num (exclude : _ list)=
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


let readNumberOfLetters dirName (imageLength: int) = 
    let fis = new System.IO.DirectoryInfo(dirName)    
    let getLettersNumber fileLength = fileLength / (int64)imageLength
    
    fis.GetFiles() 
    |> Array.map (fun fi -> fi.Name.Replace(fi.Extension, ""), (getLettersNumber fi.Length))

