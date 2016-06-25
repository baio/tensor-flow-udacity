module reader

type ReadResult<'a> = 
    | ReadSuccess of 'a
    | ReadError of string
    | ReadExit

let readConsole (validate: string -> ReadResult<'a>) : ReadResult<'a> = 
    match System.Console.ReadLine() with
    | "exit" -> ReadExit
    | str -> validate str
    
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
        
let reader = new ReaderBuilder()                 
