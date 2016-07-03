module utils

let swap (a: _[]) x y =
    let tmp = a.[x]
    a.[x] <- a.[y]
    a.[y] <- tmp

let shuffle2 rnd arr =
    arr |> Seq.iteri (fun i _ -> swap arr i (rnd i arr.Length)) 

//Shuffle array elements
let shuffle arr =
    let random = new System.Random();
    let rnd min max = random.Next(min, max)
    arr |> Seq.iteri (fun i _ -> swap arr i (rnd i arr.Length)) 
    arr

let generateShuffled (upTo : int) : int array =
        
    Seq.initInfinite (fun i -> i) 
    |> Seq.take upTo 
    |> Array.ofSeq
    |> shuffle        
    
    



