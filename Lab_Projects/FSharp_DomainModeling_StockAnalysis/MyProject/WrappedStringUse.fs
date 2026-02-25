module WrappedStringUse

open WrappedString

let s50 = WrappedString.string50 "abc" |> Option.get
printfn "s50 is %A" s50
let bad = WrappedString.string50 null
printfn "bad is %A" bad
let s100 = WrappedString.string100 "abc" |> Option.get
printfn "s100 is %A" s100
printfn "s50 is equal to s100 using module equals? %b" (WrappedString.equals s50
s100) //true
printfn "s50 is equal to s100 using Object.Equals? %b" (s50.Equals s100) //false
//printfn "s50 is equal to s100? %b" (s50 = s100) // compiler error
