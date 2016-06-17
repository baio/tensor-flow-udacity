module types


type ImageSize = {width : int; height : int}

type ImageParams = {size : ImageSize; pxielDepth : int }

type ImageInGreyScale = single [,]

type LabeledImage = {label : string; image : ImageInGreyScale}