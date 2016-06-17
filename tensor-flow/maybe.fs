module maybe
(*
let bind (optionExpression, lambda) =

     match optionExpression with
       | None -> 
           None
       | Some x -> 
           x |> lambda 
*)

let (>>=) m f = Option.bind

type MaybeBuilder() =

    member this.Bind(m, f) = Option.bind f m

    member this.Return(x) = Some x

    member this.ReturnFrom(x) = x

    member this.For(x) = Some x

let maybe = new MaybeBuilder()