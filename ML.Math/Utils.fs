module ML.Math.Utils

open System

let nextGaussian (mu : float) (sigma : float) (random: System.Random)  = 
    let u1 = random.NextDouble()
    let u2 = random.NextDouble()

    let rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2)

    mu + sigma * rand_std_normal

let nextGaussianStd : System.Random -> float = nextGaussian 0. 1. 