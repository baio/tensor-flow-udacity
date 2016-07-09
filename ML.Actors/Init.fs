module ML.Actors.Init

open Serilog

let InitLogger seqUrl = 
    let logger = (new LoggerConfiguration()).WriteTo.Seq(seqUrl).MinimumLevel.Debug().CreateLogger()
    Serilog.Log.Logger <- logger
