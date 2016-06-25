module reader

type ReadResult<'a> = 
    | ReadSuccess of 'a
    | ReadError of string
    | ReadExit

type ParseResult<'a> = 
    | ParseSuccess of 'a
    | ParseError of string

let readConsole (parse: string -> ParseResult<'a>) : ReadResult<'a> = 
    match System.Console.ReadLine() with
    | "exit" -> ReadExit
    | str -> 
        match parse str with
        | ParseSuccess value -> ReadSuccess value
        | ParseError err -> ReadError err
    
type ReaderBuilder() = 
        
    member this.Bind(m, f) = 
        match m with 
        | ReadSuccess state -> 
            f state
        | ReadError err -> 
            ReadError err
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
        b
        
let reader = new ReaderBuilder()                 
