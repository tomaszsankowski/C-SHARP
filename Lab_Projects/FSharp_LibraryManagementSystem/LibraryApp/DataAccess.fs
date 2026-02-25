module DataAccess

open System
open System.Xml.Linq
open LibraryTypes

let private getElemVal (parent: XElement) (name: string) =
    match parent.Element(XName.Get name) with
    | null -> None
    | elem -> Some elem.Value

let loadReaders (path: string) : Czytelnik list =
    let doc = XDocument.Load(path)
    
    doc.Descendants(XName.Get "Reader")
    |> Seq.map (fun r ->
        let id = int (r.Attribute(XName.Get "Id").Value)
        let deposit = decimal (r.Attribute(XName.Get "DepositUSD").Value)
        let joinDate = DateTime.Parse(r.Attribute(XName.Get "JoinDate").Value)
        let statusStr = r.Attribute(XName.Get "Status").Value
        let status = if statusStr = "Premium" then Premium else Standard
        
        let contactInfoElem = r.Element(XName.Get "ContactInfo")
        let contactDetails = 
            match contactInfoElem with
            | null -> None
            | ci ->
                let imie = ci.Element(XName.Get "FirstName").Value
                let nazwisko = ci.Element(XName.Get "LastName").Value
                let dob = DateTime.Parse(ci.Element(XName.Get "DOB").Value)
                let nrKarty = ci.Element(XName.Get "CardNumber").Value
                let email = getElemVal ci "Email"
                
                let phone = getElemVal ci "Phone"
                let addr = getElemVal ci "Address"
                
                let adresowe = 
                    match phone, addr with
                    | Some p, Some a -> TelefonIAdres(p, a)
                    | Some p, None -> Telefon(p)
                    | None, Some a -> Adres(a)
                    | None, None -> failwith "Nieprawidłowy stan: Musi być telefon lub adres!"

                Some {
                    Imie = imie; Nazwisko = nazwisko; DataUrodzenia = dob;
                    NrKarty = nrKarty; Email = email; SzczegolyAdresowe = adresowe
                }

        {
            Id = id
            DaneOpisowe = contactDetails
            Kaucja = deposit * 1.0m<USD>
            DataDolaczenia = joinDate
            Status = status
            SaldoKar = 0.0m<PLN>
        }
    )
    |> Seq.toList

let loadLoans (path: string) : (int * Wypozyczenie list) list =
    let doc = XDocument.Load(path)
    
    doc.Descendants(XName.Get "ReaderLoans")
    |> Seq.map (fun rl ->
        let rId = int (rl.Attribute(XName.Get "ReaderId").Value)
        let loans = 
            rl.Elements(XName.Get "Loan")
            |> Seq.map (fun l -> 
                let isbn = l.Attribute(XName.Get "ISBN").Value
                let returned = bool.Parse(l.Attribute(XName.Get "Returned").Value)
                (isbn, returned)
            )
            |> Seq.toList
        (rId, loans)
    )
    |> Seq.toList