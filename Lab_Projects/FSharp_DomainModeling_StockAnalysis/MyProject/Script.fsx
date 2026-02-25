let y = 0

// Sum of squares for integers

let dataI = [1;2;3;4]
let sqrI (x:int) = x * x
let sumOfSquaresI nums =
    let mutable acc = 0
    for n in nums do
        acc <- acc + sqrI n
    acc
sumOfSquaresI dataI;;

// Sum of squares for floats

let dataF = [1.;2.;3.;4.]
let sqrF (x:float) = x * x
let sumOfSquaresF nums =
    let mutable acc = 0.
    for n in nums do
        acc <- acc + sqrF n
    acc
sumOfSquaresF dataF;;

// Non generic functional for floats

let rec sumOfSquaresFun nums =
    match nums with
    | [] -> 0.
    | h::t -> sqrF h + sumOfSquaresF t
sumOfSquaresFun dataF;;

// Generic functional

let inline sqr x = x * x
let inline sumOfSquaresFunGen nums =
    let mutable acc = LanguagePrimitives.GenericZero
    for n in nums do
        acc <- acc + sqr n
    acc
sumOfSquaresFunGen dataI;;
sumOfSquaresFunGen dataF;;

// Recursive generic
let sumOfSquares nums =
    nums
    |> Seq.map(fun x -> x * x)
    |> Seq.sum

// Recursive generic parallel
open FSharp.Collections.ParallelSeq
let sumOfSquaresPar nums =
    nums
    |> PSeq.map(fun x -> x * x)
    |> PSeq.sum

// Get financial data about Microsoft (msft.us)
open System.Net.Http
open XPlot.Plotly
let loadPrices (ticker: string) =
    let stooqTicker = ticker.ToLower() + ".us"
    let dataStart = System.DateTime(2000,1,1).ToString("yyyyMMdd")
    let dataEnd = System.DateTime.Now.ToString("yyyyMMdd")
    let url = $"https://stooq.com/q/d/l/?s={stooqTicker}&d1={dataStart}&d2={dataEnd}&i=d"

    let client = new HttpClient()
    let getDataAsync =
        async {
            let! csvResponse = Async.AwaitTask (client.GetStringAsync(url))
            return csvResponse
        }

    let csv = Async.RunSynchronously getDataAsync

    let prices =
        csv.Split([|'r';'\n'|], System.StringSplitOptions.RemoveEmptyEntries)
        |> Seq.skip 1
        |> Seq.map (fun line -> line.Split(','))
        |> Seq.filter (fun values -> values.Length = 6)
        |> Seq.map (fun values -> (values.[0], values.[4]))
        |> Seq.toArray

    prices

Chart.Line(loadPrices "MSFT").Show();; // historia cen akcji dla firmy Microsoft
Chart.Line(loadPrices "ORCL").Show();; // historia cen akcji dla firmy Oracle
Chart.Line(loadPrices "EBAY").Show();; // historia cen akcji dla firmy Ebay

["MSFT"; "ORCL"; "EBAY"] |> Seq.iter (fun ticker ->
    let data = loadPrices ticker
    Chart.Line(data).Show()
);;

let loadPricesAsync (ticker: string) = async {
    let stooqTicker = ticker.ToLower() + ".us"
    let dataStart = System.DateTime(2000,1,1).ToString("yyyyMMdd")
    let dataEnd = System.DateTime.Now.ToString("yyyyMMdd")
    let url = $"https://stooq.com/q/d/l/?s={stooqTicker}&d1={dataStart}&d2={dataEnd}&i=d"

    let client = new HttpClient()
    let! csv = Async.AwaitTask (client.GetStringAsync(url))

    let prices =
        csv.Split([|'r';'\n'|], System.StringSplitOptions.RemoveEmptyEntries)
        |> PSeq.ofArray
        |> PSeq.skip 1
        |> PSeq.map (fun line -> line.Split(','))
        |> PSeq.filter (fun values -> values.Length = 6)
        |> PSeq.map (fun values -> (values.[0], values.[4]))
        |> PSeq.toArray

    return prices
}

let requests =
    [
        loadPricesAsync "MSFT"
        loadPricesAsync "ORCL"
    ]

let parallelRequests = Async.Parallel requests

let results = Async.RunSynchronously parallelRequests
results |> Array.iter (fun data ->
    Chart.Line(data).Show()
)

type Adder = int -> int
type AdderGenerator = int -> Adder

let a:AdderGenerator = fun x -> (fun y -> x + y)
// let b:AdderGenerator = fun (x:float) -> (fun y -> x + y)
let c = fun (x:float) -> (fun y -> x + y)
