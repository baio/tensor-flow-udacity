module config

let BASE_PATH = "C:/dev/tensor-flow-udacity/"
let LETTERS_PATH = BASE_PATH + "data/letters/"
let DATA_PATH = BASE_PATH + ".data/"

let getLetterPath letter = LETTERS_PATH + letter + ".png"
let getDataPath path = DATA_PATH + path