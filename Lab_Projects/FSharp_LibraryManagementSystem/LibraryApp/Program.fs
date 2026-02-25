module Program

open System
open System.Net.Http
open System.Xml.Linq
open System.Globalization
open LibraryTypes
open DataAccess

let getUsdToPlnRate () =
    try
        use client = new HttpClient()
        let url = "https://api.nbp.pl/api/exchangerates/rates/a/usd/?format=xml"
        let xmlString = client.GetStringAsync(url).Result
        let doc = XDocument.Parse(xmlString)
        let midElement = doc.Descendants(XName.Get "Mid") |> Seq.head
        Decimal.Parse(midElement.Value, CultureInfo.InvariantCulture)
    with
    | ex -> 
        printfn "Błąd pobierania kursu z NBP (XML): %s" ex.Message
        printfn "Używam domyślnego kursu 4.0m."
        4.0m


[<EntryPoint>]
let main argv =
    printfn "=== System Biblioteczny ==="
    
    printf "Pobieranie kursu USD/PLN... "
    let usdToPlnRate = getUsdToPlnRate ()
    printfn "Gotowe."
    printfn "Aktualny kurs (Mid): %.4f PLN" usdToPlnRate

    let readersPath = "data/readers.xml" 
    let loansPath = "data/loans.xml"

    let readers = loadReaders readersPath
    let loans = loadLoans loansPath
    let lib = new Library(readers, loans)

    let mutable running = true

    while running do
        printfn "\n--- MENU ---"
        printfn "1. Wyświetl informacje o czytelniku"
        printfn "2. Dodaj karę (PLN)"
        printfn "3. Sprawdź czy kary (PLN) przekraczają kaucję (USD)"
        printfn "4. Spróbuj awansować na Premium"
        printfn "0. Wyjdź"
        printf "Wybór: "
        
        match Console.ReadLine() with
        | "1" ->
            printf "Podaj ID czytelnika: "
            match Int32.TryParse(Console.ReadLine()) with
            | true, id ->
                match lib.GetReader id with
                | Some r -> 
                    printfn "--- Dane Czytelnika ---"
                    printfn "ID: %d | Status: %A" r.Id r.Status
                    printfn "Data dołączenia: %s" (r.DataDolaczenia.ToShortDateString())

                    let depositPln = (decimal r.Kaucja) * usdToPlnRate
                    printfn "--- Stan Konta ---"
                    printfn "Kaucja: %.2f USD (ok. %.2f PLN)" r.Kaucja depositPln
                    printfn "Kary:   %.2f PLN" r.SaldoKar

                    printfn "--- Dane Kontaktowe ---"
                    match r.DaneOpisowe with
                    | Some d -> 
                         printfn "Imię: %s | Nazwisko: %s" d.Imie d.Nazwisko
                         let emailStr = match d.Email with Some e -> e | None -> "brak"
                         printfn "Email: %s" emailStr
                         match d.SzczegolyAdresowe with
                         | Telefon t -> printfn "Kontakt: Tel %s" t
                         | Adres a -> printfn "Kontakt: Adres %s" a
                         | TelefonIAdres (t, a) -> printfn "Kontakt: Tel %s, Adres %s" t a
                    | None -> printfn "Brak danych osobowych."
                | None -> printfn "Nie znaleziono czytelnika."
            | _ -> printfn "Błąd ID."

        | "2" ->
            printf "Podaj ID czytelnika: "
            let idResult = Int32.TryParse(Console.ReadLine())
            printf "Podaj kwotę kary (PLN): "
            let amountResult = Decimal.TryParse(Console.ReadLine())
            
            match idResult, amountResult with
            | (true, id), (true, amount) ->
                lib.AddFine id (amount * 1.0m<PLN>)
                printfn "Zaktualizowano saldo."
            | _ -> printfn "Błąd danych."

        | "3" ->
            printf "Podaj ID czytelnika: "
            match Int32.TryParse(Console.ReadLine()) with
            | true, id ->
                match lib.GetReader id with
                | Some r ->
                    let depositPln = (decimal r.Kaucja) * usdToPlnRate
                    let finesPln = decimal r.SaldoKar
                    
                    printfn "Kaucja: %.2f USD (ok. %.2f PLN po kursie %.4f)" r.Kaucja depositPln usdToPlnRate
                    printfn "Kary:   %.2f PLN" finesPln
                    
                    if finesPln > depositPln then
                        printfn "UWAGA: Kary przekraczają depozyt!"
                    else
                        printfn "Status OK: Depozyt pokrywa kary."
                | None -> printfn "Nie znaleziono czytelnika."
            | _ -> printfn "Błąd ID."

        | "4" ->
            printf "Podaj ID: "
            match Int32.TryParse(Console.ReadLine()) with
            | true, id ->
                if lib.TryPromoteToPremium id then
                    printfn "Awansowano czytelnika na Premium!"
                else
                    printfn "Czytelnik nie spełnia warunków awansu."
            | _ -> printfn "Błąd ID."

        | "0" -> running <- false
        | _ -> printfn "Nieznana opcja."

    0