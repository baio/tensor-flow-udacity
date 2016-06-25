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

type WorkerCommand = 
    | StartWorker of InputOutputPaths

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
        | IsFileUri(name) -> ParseError <| name
        | _ -> ParseError("Path not found")
    
let readConsoleInput() : ReadResult<InputBasket> = 
    reader {        

        printfn "Please enter required parameters. You always welcome type 'exit' to quit"
        
        printfn "In order to read images, type directory path and files extensions using comma separator (optional). Default %s (press enter)" DEFAULT_INPUT_DIR                    
        let! dir = readConsole parseInputDir
        let withInput = InputBasketWithInput dir
        yield withInput
                       
        printfn "In order to store data, type output file name. Default %s (press enter)" DEFAULT_OUTPUT_FILE            
        let! file = readConsole parseOutputFile 
        yield InputBasketWithInputOutput {input = dir; output = file }
    }
                                
let inputBasketActor (filePath:string) (reporter:IActorRef) (mailbox:Actor<_>) =
           
    let rec input () = 
        actor {         
            let! message = mailbox.Receive();
            match message with 
            | Start ->
                mailbox.Self <! ReadInput
                return! input()
            | ReadInput ->
                match readConsoleInput() with
                | ReadSuccess io -> 
                    //select "user/workerActor" mailbox.Context.System <! StartWorker io
                    return! input()                                    
                | _ ->
                    return! input()                                    
        }

    input()

