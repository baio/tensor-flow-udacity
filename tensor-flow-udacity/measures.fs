module measures

// imagePixels = 3 (R, G, B) * 4 (4 bytes per color) * imageBytes

[<Measure>]
type imageByte =
    static member ConvertToPixel(x: int<imageByte>) =  x / 12<1 / imagePixel * imageByte>
and [<Measure>] imagePixel =
    static member ConvertToByte(x: int<imagePixel>) =  x * 12<imageByte / imagePixel>