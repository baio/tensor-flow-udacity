module ML.Actors.InputActor

open System
open Akka.Actor
open Akka.FSharp

open Types
open DataProccessing.Reader
open DataProccessing.Types

type InputCommand = 
    | Start 
    | ReadInput
   

type InputBasket = 
    | InputBasketEmpty
    | InputBasketWithInput of input : DirPath 
    | InputBasketWithInputOutput of InputOutputPaths

let (|IsDirUri|_|) path = if System.IO.Directory.Exists path then Some path else None
let (|IsFileUri|_|) path = if System.IO.File.Exists path then Some path else None

let DEFAULT_INPUT_DIR = @"C:\dev\.data\notMNIST_small"
let DEFAULT_INPUT_EXT = "png"
let DEFAULT_OUTPUT_FILE  = @"C:\dev\.data\notMNIST_normalized.csv"
    
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
            printfn "In order to read images, type directory path and files extensions using comma separator (optional). Default %s (press enter), .%s" DEFAULT_INPUT_DIR  DEFAULT_INPUT_EXT
            let! dir = readConsole parseInputDir
            yield! readConsoleInput <| InputBasketWithInput dir
        | InputBasketWithInput dir ->                        
            printfn "In order to store data, type output file name. Default %s (press enter)" DEFAULT_OUTPUT_FILE            
            let! file = readConsole parseOutputFile 
            yield! readConsoleInput <| InputBasketWithInputOutput {input = dir; output = file }
        | InputBasketWithInputOutput _ ->
            yield basket                        
    }
                                
let InputActor (routerActor : IActorRef) (mailbox:Actor<InputCommand>) =
           
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
                    match io with 
                    | InputBasketWithInputOutput paths ->
                        logInfo mailbox <| sprintf "Input success %A" paths
                        routerActor <! RWStart paths
                    |_ ->
                        logWarning mailbox "Thats strange" 
                        return! input(InputBasketEmpty)                                    
                | ReadError(err, Some(io)) ->                     
                    logWarning mailbox <| sprintf "Input error %s ; %A" err io
                    mailbox.Self <! ReadInput
                    return! input(io)           
                | ReadExit ->
                    mailbox.Context.System.Terminate() |> ignore                                            
                | _ ->
                    return! input prevIO                                    
        }

    input InputBasketEmpty

