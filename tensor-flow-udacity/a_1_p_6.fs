module a_1_p_6

(*
Problem 6
Let's get an idea of what an off-the-shelf classifier can give you on this data. It's always good to check that there is something to learn, 
and that it's a problem that is not so trivial that a canned solution solves it.
Train a simple model on this data using 50, 100, 1000 and 5000 training samples. Hint: you can use the LogisticRegression model from sklearn.linear_model.
Optional question: train an off-the-shelf model on all the data!
*)

(*
train_dataset_lin = train_dataset.reshape(train_dataset.shape[0],train_dataset.shape[1]*train_dataset.shape[2])
test_dataset_lin = test_dataset.reshape(test_dataset.shape[0],test_dataset.shape[1]*test_dataset.shape[2])
valid_dataset_lin = valid_dataset.reshape(valid_dataset.shape[0],valid_dataset.shape[1]*valid_dataset.shape[2])

score_train=[]
score_test=[]
score_valid=[]
numel_vector = [50, 100, 250, 500, 1000, 2500, 5000, 10000]
for numel in numel_vector:
    logreg = LogisticRegression() #C=1e5 C=0.001
    idx = random.sample(range(len(train_labels)),numel)
    logreg.fit(train_dataset_lin[idx,:], train_labels[idx])
              
    res = logreg.predict(train_dataset_lin)
    score_train.append( sklearn.metrics.accuracy_score(train_labels, res) )

    res = logreg.predict(test_dataset_lin)
    score_test.append( sklearn.metrics.accuracy_score(test_labels, res) )

    res = logreg.predict(valid_dataset_lin)
    score_valid.append( sklearn.metrics.accuracy_score(valid_labels, res) )
    
    print(score_train)
    print(score_test)
    print(score_valid)
*)

//Logistic classifier

//Linear function, giant index multiplier

// WX + b = Y
// 
//                [a b 1]    [y1]
// [w1, w2, w3] * [c d 1] =  [y2]
//                [e f 1]    [y3]

// (n, k) * (k, m) = (n, m)
// (1, 3) * (3, 3) = (1, 3)

// W - weights
// X - inputs
// b - bias
// y - scores, logits
// W, b - unknown (train)

// Scores -> Probabilities, use softmax
// S(Yi) = e^(Yi) / sum(e^Yi)

open measures
open utils

open MathNet.Numerics

let softmax (x: float[][]) =     
    
    let ePow x = System.Math.Pow(System.Math.E, x)
    
    let softmax' (xs': float[]) (x': float) = 
      (ePow  x') /  (xs' |> Seq.map ePow |> Seq.sum)
    
    x |> Seq.map (fun m -> m |> Seq.map (softmax' m)) 

let lettersToScores(letters: string[]) = 
    
    //let arr = [|"a"; "b"; "c"; "d"; "e"; "f"; "j"; "i"|];

    let getIndex letter = int letter - int "a"

    letters |> Array.map getIndex 

let calcLinear (x: float[][]) (y: float[]) = 

    Fit.LinearMultiDim(x, y)
       
    
let readData fileName = 
    
    use file = new System.IO.StreamReader(new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))

    let t = file.ReadToEnd();
    let bytes = file.ReadToEnd().ToCharArray() |> Array.map byte;

    bytes 
    |> Seq.chunkBySize (int (imagePixel.ConvertToByte IMAGE_LENGTH))
    |> Seq.map getPixels
        
let readLabels fileName = 
    
    use file = new System.IO.StreamReader(new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
    file.ReadToEnd().ToCharArray() |> Seq.map System.Char.ToString;
        




    

