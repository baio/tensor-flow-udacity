module types


type ImageSize = {width : int; height : int}

type ImageParams = {size : ImageSize; pxielDepth : int }

type ImageInGreyScale = single [,]

type LabeledImage = {label : string; image : ImageInGreyScale}

type DirPath = 
    | DirPath of string
    | DirPathFilter of string * string


type InputOutputPaths = {input : DirPath; output : string}
    