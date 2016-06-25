module maybe

type MaybeBuilder() =

    member this.Bind(m, f) = Option.bind f m

    member this.Return(x) = Some x

    member this.ReturnFrom(x) = x
    

let maybe = new MaybeBuilder()
