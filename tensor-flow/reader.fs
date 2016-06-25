module reader

type ReadResult<'a, 'b> = 
    | ReadSuccess of 'a
    | ReadError of string * 'b option
    | ReadExit

type ParseResult<'a> = 
    | ParseSuccess of 'a
    | ParseError of string

let readConsole (parse: string -> ParseResult<'a>) : ReadResult<'a, 'b> = 
    match System.Console.ReadLine() with
    | "exit" -> ReadExit
    | str -> 
        match parse str with
        | ParseSuccess value -> ReadSuccess value
        | ParseError err -> ReadError(err, None)
    
type ReaderBuilder() = 
        
    member this.Bind(m, f) = 
        match m with 
        | ReadSuccess state -> 
            f state
        | ReadError(err, state) -> 
            ReadError(err, state)
        | ReadExit -> 
            ReadExit

    member this.Zero()  = 
        ReadExit

    member this.Return x = 
        ReadSuccess x

    member this.Yield x = 
        ReadSuccess x

    member this.YieldFrom x = 
        x
            
    member this.Delay(f) = f()

    member this.Combine (a, b) = 
        match (a, b) with
        //previous some, latest exit
        | (_, ReadExit) -> ReadExit
        // both success, take latest
        | (ReadSuccess(_), ReadSuccess(value)) -> ReadSuccess(value)
        // previous success, next error, new error with previous state
        | (ReadSuccess(value), ReadError(err, None)) -> ReadError(err, Some value)        
        // it was an error which is bubble up now
        | (ReadSuccess(_), ReadError(err, Some(value))) -> ReadError(err, Some value)        
        // all other cases is impossible
        | (a, b ) -> 
            raise(System.Exception("Unreachable code"))
        
let reader = new ReaderBuilder()                 
