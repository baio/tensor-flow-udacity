module types

open DataProccessing.Types

type IORouterMessages = 
    | IORouterStart of InputOutputPaths
    | IORouterWriteComplete
    | IORouterWriterClosed 

    

