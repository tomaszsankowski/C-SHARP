#load "WrappedString.fs"
#load "EmailAddress.fs"
#load "ContactInfo.fs"

open EmailAddress
open ContactInfo

let sampleEmail = (EmailAddress.create "test@example.com") |> Option.get
let samplePostAddress = "ul. Gabriela Narutowicza 11/12, 80-222 Gdañsk"

let contact1 = createEmailOnly sampleEmail
print contact1

let contact2 = createPostOnly samplePostAddress
print contact2

let contact3 = createEmailAndPost sampleEmail samplePostAddress
print contact3
