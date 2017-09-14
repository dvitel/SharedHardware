open System

let fib = 
    let rec fibI a1 a2 =
        seq {
            yield a1
            yield! fibI a2 (a1 + a2)
        }
    fibI 1uL 1uL

let rec inc flag: unit = 
    inc (not flag)

printfn "Do calculus"
//fib |> Seq.take 1000000 |> Seq.toArray |> printfn "%A"
[
    for i in 1..8 ->
        async { inc false }
] |> Async.Parallel |> Async.RunSynchronously

//try 
//let hugeArray = 
//    [for i in 1uL..1024uL*1024uL*50uL -> i]

//let rec prnt(): unit = 
//    hugeArray |> List.iter(printfn "%d")
//    prnt()

//prnt()
//with e -> 
//    printfn "%A" e


