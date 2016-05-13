module a_1_p_2.flow

open config

open utils
open a_1_p_1

let createLetterBinaries _ = createAlphabitBinaries (getDataPath "notMNIST_small")  (getDataPath "out/letters-bl")

let showLetterImagesFromBinaries showImages = readFileAndShowImages showImages 30 (getDataPath "out/letters-bl/A.bl") 