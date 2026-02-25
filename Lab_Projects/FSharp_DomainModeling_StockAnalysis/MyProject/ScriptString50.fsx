#load "String50.fs"

let notSoLongString = String50.create "notSoLongString"
let toolongString = String50.create "tooLongLongLongLongLongLongLongLongLongLongLongLongLongString"

match toolongString with
| Some sbyte -> printfn "good"
| None -> printfn "bad"

notSoLongString
|> Option.map String50.value
|> Option.map (fun s -> s.ToUpper())
|> Option.iter (printfn "%s")

match notSoLongString with
| Some s -> printf "%s" ((String50.value s).ToUpper())
| None -> ()

let result = match notSoLongString with
                | Some s -> (String50.value s).ToUpper()
                | None -> ""
printfn "%s" result
