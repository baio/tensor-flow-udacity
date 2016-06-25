module Actors

open System
open Akka.Actor
open Akka.FSharp
open types
open reader

type InputCommand = 
    | Start 
    | ReadInput
   
type InputOutputPaths = {input : DirPath; output : string }

type InputBasket = 
    | InputBasketEmpty
    | InputBasketWithInput of input : DirPath 
    | InputBasketWithInputOutput of InputOutputPaths

let (|IsDirUri|_|) path = if System.IO.Directory.Exists path then Some path else None
let (|IsFileUri|_|) path = if System.IO.File.Exists path then Some path else None

let DEFAULT_INPUT_DIR = "c:/dev/.data/tfu/in/images"
let DEFAULT_OUTPUT_FILE  = "c:/dev/.data/tfu/out/images.data"
    
let parseInputDir (str: string) : ParseResult<DirPath> =
        
    match str.Split([|','|]) with
    | [|name|] when String.IsNullOrEmpty(name) ->
        ParseSuccess <| DirPath DEFAULT_INPUT_DIR
    | [|name|] -> 
        match (string name).Trim() with
        | IsDirUri(name) -> ParseSuccess <| DirPath name
        | _ -> ParseError("Path not found")
    | [|name; ext|] ->
        match (string name).Trim() with
        | IsDirUri(name) ->  ParseSuccess <| DirPathFilter (name, ext.Trim())
        | _ -> ParseError("Path not found")
    | _ -> ParseError("Unrecognized input")

let parseOutputFile (str: string) : ParseResult<string> =
        
    match str with
    | name when String.IsNullOrEmpty(name) ->
        ParseSuccess <| DEFAULT_OUTPUT_FILE
    | name -> 
        match (string name).Trim() with
        | IsFileUri(name) -> 
            ParseSuccess name
        | _ -> ParseError("Path not found")
    
let rec readConsoleInput(basket: InputBasket) : ReadResult<InputBasket, InputBasket> = 
    reader {        
        //store latest successful basket
        yield basket
        match basket with 
        | InputBasketEmpty ->
            printfn "Please enter required parameters. You always welcome type 'exit' to quit"        
            printfn "In order to read images, type directory path and files extensions using comma separator (optional). Default %s (press enter)" DEFAULT_INPUT_DIR                    
            let! dir = readConsole parseInputDir
            yield! readConsoleInput <| InputBasketWithInput dir
        | InputBasketWithInput dir ->                        
            printfn "In order to store data, type output file name. Default %s (press enter)" DEFAULT_OUTPUT_FILE            
            let! file = readConsole parseOutputFile 
            yield! readConsoleInput <| InputBasketWithInputOutput {input = dir; output = file }
        | InputBasketWithInputOutput _ ->
            yield basket                        
    }
                                
let inputBasketActor (mailbox:Actor<_>) =
           
    let rec input (prevIO) = 
        actor {         
            let! message = mailbox.Receive();
            match message with 
            | Start ->
                mailbox.Self <! ReadInput
                return! input(prevIO)
            | ReadInput ->
                match readConsoleInput(prevIO) with
                | ReadSuccess io -> 
                    printfn "Success %A" io
                    mailbox.Self <! Start
                    return! input(InputBasketEmpty)                                    
                | ReadError(err, Some(io)) ->                     
                    printfn "Error %s ; %A" err io
                    mailbox.Self <! ReadInput
                    return! input(io)           
                | ReadExit ->
                    mailbox.Context.System.Terminate() |> ignore                                            
                | _ ->
                    return! input prevIO                                    
        }

    input InputBasketEmpty

