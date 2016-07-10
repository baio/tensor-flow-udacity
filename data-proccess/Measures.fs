module DataProccessing.Measures

[<Measure>]
type imageBytes =
    static member ConvertToPixel(x: int<imageBytes>) =  x / 12<1 / imageBytes * imagePixels>
/// 1 pixel = 12 bytes (R, G, B), 4 byte for each color (float = 4 bytes)
and [<Measure>] imagePixels =
    static member ConvertToByte(x: int<imageBytes>) =  x * 12<imageBytes / imageBytes>

[<Measure>] 
type percent
