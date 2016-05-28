module types

type TTVSets<'a>  = ('a list) * ('a list) * ('a list)
type TTVPermutes<'a> = string * TTVSets<'a>
