module Actors

open System
open Akka.Actor
open Akka.FSharp
open types

type InputCommand = 
    | Start 
    | ReadInput
   
type Result<'a> = 
    | Success of 'a
    | Error of string

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
    
let parseInputDir (str: string) : Result<DirPath> =
        
    match str.Split([|','|]) with
    | [|name|] when String.IsNullOrEmpty(name) ->
        Success <| DirPath DEFAULT_INPUT_DIR
    | [|name|] -> 
        match (string name).Trim() with
        | IsDirUri(name) -> Success <| DirPath name
        | _ -> Error("Path not found")
    | [|name; ext|] ->
        match (string name).Trim() with
        | IsDirUri(name) ->  Success <| DirPathFilter (name, ext.Trim())
        | _ -> Error "Path not found"
    | _ -> Error "Unrecognized input"


let parseOutputFile (str: string) : Result<string> =
        
    match str with
    | name when String.IsNullOrEmpty(name) ->
        Success <| DEFAULT_OUTPUT_FILE
    | name -> 
        match (string name).Trim() with
        | IsFileUri(name) -> Success <| name
        | _ -> Error("Path not found")
    
                            
let inputBasketActor (filePath:string) (reporter:IActorRef) (mailbox:Actor<_>) =
        
    let getInput parser =
        let line = Console.ReadLine()
        match line.ToLower() with
        | "exit" ->
             mailbox.Context.System.Terminate() |> ignore
             ""
        | line -> line

    let rec readInput basket = 
        match basket with
        | InputBasketEmpty ->
            printfn "In order to read images, type directory path and files extensions using comma separator (optional). Default %s (press enter)" DEFAULT_INPUT_DIR
            match parseInputDir <| getInput() with
                | Success dir -> 
                    readInput <| InputBasketWithInput dir
                | Error msg ->
                    //TODO
                    readInput basket
        | InputBasketWithInput dir ->
            printfn "In order to store data, type output file name. Default %s (press enter)" DEFAULT_OUTPUT_FILE
            match parseOutputFile <| getInput() with
                | Success file -> 
                    InputBasketWithInputOutput {input = dir; output = file}
                | Error msg ->
                    //TODO
                    readInput basket
        | InputBasketWithInputOutput _ -> raise(Exception("Unreachable code"))


    let rec input () = 
        actor {         
            let! message = mailbox.Receive();
            match message with 
            | Start ->
                printfn "Please enter required parameters. You always welcome type 'exit' to quit"
                mailbox.Self <! ReadInput
                return! input()
            | ReadInput ->
                match readInput InputBasketEmpty with
                | InputBasketWithInputOutput io -> 
                    select "user/workerActor" mailbox.Context.System <! StartWorker io
                    return! input()                                    
                | _ ->
                    return! input()                                    
        }

    input()

