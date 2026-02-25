#load "WrappedString.fs"
#load "EmailAddress.fs"

let address1 = EmailAddress.create "x@example.com" //x@example.com
let address2 = EmailAddress.create "example.com" //null

let success (EmailAddress.EmailAddress s) = printfn "success creating email %s" s
let failure msg = printfn "error creating email: %s" msg
let createEmailAddress = EmailAddress.createWithCont success failure

let address3 = createEmailAddress "example.com"
let address4 = createEmailAddress "x@example.com"
