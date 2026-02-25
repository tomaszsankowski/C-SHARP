module LibraryTypes

open System

[<Measure>] type USD
[<Measure>] type PLN

type DaneAdresowe =
    | Telefon of string
    | Adres of string
    | TelefonIAdres of string * string

type DaneKontaktowe = {
    Imie : string
    Nazwisko : string
    DataUrodzenia : DateTime
    NrKarty : string
    Email : string option
    SzczegolyAdresowe : DaneAdresowe
}

type StatusKonta = Standard | Premium

type Wypozyczenie = string * bool

type Czytelnik = {
    Id : int
    DaneOpisowe : DaneKontaktowe option
    Kaucja : decimal<USD>
    DataDolaczenia : DateTime
    mutable Status : StatusKonta
    mutable SaldoKar : decimal<PLN>
}

type Library(readers: Czytelnik list, loanHistory: (int * Wypozyczenie list) list) =
    
    let loansMap = Map.ofList loanHistory
    let readersMap = readers |> List.map (fun r -> r.Id, r) |> Map.ofList

    member this.GetLoanHistory(readerId: int) =
        match loansMap.TryFind readerId with
        | Some history -> history
        | None -> []

    member this.IsPatronForLongerThan(readerId: int) (days: int) =
        match readersMap.TryFind readerId with
        | Some reader -> 
            let duration = DateTime.Now - reader.DataDolaczenia
            duration.TotalDays > float days
        | None -> false

    member this.AddFine(readerId: int) (amount: decimal<PLN>) =
        match readersMap.TryFind readerId with
        | Some reader -> 
            reader.SaldoKar <- reader.SaldoKar + amount
        | None -> ()

    member this.GetReader(readerId: int) = 
        readersMap.TryFind readerId

    member this.TryPromoteToPremium(readerId: int) =
        let minLoans = 5
        let minDays = 365
        
        match readersMap.TryFind readerId with
        | Some reader when reader.Status = Standard ->
            let history = this.GetLoanHistory readerId
            let isLongTerm = this.IsPatronForLongerThan readerId minDays
            
            if List.length history > minLoans && isLongTerm then
                reader.Status <- Premium
                true
            else 
                false
        | _ -> false