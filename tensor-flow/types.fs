module types


type ImageSize = {width : int; height : int}

type ImageParams = {size : ImageSize; pxielDepth : int }

type ImageInGreyScale = single [,]
type ImageInBW = byte [,]

type LabeledImage = {label : string; image : ImageInBW}

type DirPath = 
    | DirPath of string
    | DirPathFilter of string * string


type InputOutputPaths = {input : DirPath; output : string}

type IORouterMessages = 
    | IORouterStart of InputOutputPaths
    | IORouterWriteComplete

    

