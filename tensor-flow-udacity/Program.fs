module main

open Fuchu

open a_1_p_1;
open a_1_p_2;
//open r_provider_test;

[<EntryPoint>]
let main args = 
    
    //createAlphabitBinaries "../../../data/letters"  "../../../data/out/letters-bl"    
    readDirectoryAndShowImages 3 "../../../data/out/letters-bl"
    1
    //let res = display_image()
    //let res = defaultMainThisAssembly args 
    //System.Console.ReadKey() |> ignore;
    //defaultMainThisAssembly args 
    //readFileAndShowImages "../../../data/out/letters-bl/A.bl" 3
    //1
    //defaultMainThisAssembly args 
