module r_provider_test

open System
open RDotNet
open RProvider
open RProvider.graphics
open RProvider.stats


let default_test () = 
    // Random number generator
    let rng = Random()
    let rand () = rng.NextDouble()

    // Generate fake X1 and X2 
    let X1s = [ for i in 0 .. 9 -> 10. * rand () ]
    let X2s = [ for i in 0 .. 9 -> 5. * rand () ]

    // Build Ys, following the "true" model
    let Ys = [ for i in 0 .. 9 -> 5. + 3. * X1s.[i] - 2. * X2s.[i] + rand () ]


    let dataset = namedParams [ "Y", box Ys; "X1", box X1s; "X2", box X2s; ] |> R.data_frame
    
    let result = R.lm(formula = "Y~X1+X2", data = dataset)

    R.plot result


let display_image () = 
    let c = [ 1; 0; 0; 1];
    let mx = namedParams [ "data", box c; "nrow", box 2; "ncol", box 2; ] |> R.matrix
    R.image mx 
