module ContactInfo

open EmailAddress
open WrappedString

type T =
    | EmailOnly of EmailAddress.T
    | PostOnly of string
    | EmailAndPost of EmailAddress.T * string

let createEmailOnly email =
    EmailOnly email

let createPostOnly postAddress =
    PostOnly postAddress

let createEmailAndPost email postAddress =
    EmailAndPost (email, postAddress)

let print contactInfo =
    match contactInfo with
    | EmailOnly email ->
        printfn "Contact via Email: %A" (email)
    | PostOnly post ->
        printfn "Contact via Post: %A" (post)
    | EmailAndPost (email, post) ->
        printfn "Contact via Email: %A and Post: %A" (email) (post)
