module LibraryTypes

[<Measure>]
type USD

[<Measure>]
type PLN

type DaneAdresowe =
    | Telefon of string
    | Adres of string
    | TelefonIAdres of string * string

type DaneKontaktowe =
    {
      Imie: string
      Nazwisko: string
      DataUrodzenia: System.DateTime
      NrKarty: string
      Email: string option
      SzczegolyAdresowe: DaneAdresowe
    }

type StatusKonta =
    | Standard
    | Premium

type Wypozyczenie = string * bool

type Czytelnik =
    {
      Id: int
      DaneOpisowe: DaneKontaktowe option
      Kaucja: decimal<USD>
      DataDolaczenia: System.DateTime
      mutable Status: StatusKonta
      mutable SaldoKar: decimal<PLN>
    }

type Library =
    
    new: readers: Czytelnik list * loanHistory: (int * Wypozyczenie list) list ->
           Library
    
    member AddFine: readerId: int -> amount: decimal<PLN> -> unit
    
    member GetLoanHistory: readerId: int -> Wypozyczenie list
    
    member GetReader: readerId: int -> Czytelnik option
    
    member IsPatronForLongerThan: readerId: int -> days: int -> bool
    
    member TryPromoteToPremium: readerId: int -> bool