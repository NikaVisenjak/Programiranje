# Bolnišnica - rešitev izpita (Programiranje - praktični del, 1. izpit, 28.1.2026)

## Struktura projekta

```
BolnisnicaApp/
├── BolnisnicaApp.csproj
├── Program.cs                       <- vstopna točka (Naloga 4 in 5)
├── Models/
│   ├── Zaposleni.cs                 <- abstraktni razred (Naloga 1)
│   ├── Zdravnik.cs                  <- Naloga 1 in 2
│   ├── MedicinskaSestra.cs          <- Naloga 1 in 6
│   ├── Pacient.cs                   <- Naloga 1
│   ├── Bolnisnica.cs                <- Naloga 1
│   ├── Direktor.cs                  <- Singleton (Naloga 3)
│   └── Naloga.cs                    <- pomožni razred za delovne naloge
├── Interfaces/
│   └── IZadolzen.cs                 <- vmesnik (Naloga 6)
└── Extensions/
    └── BolnisnicaExtensions.cs      <- razširitvena metoda (Naloga 7)
```

Razredi so razdeljeni v ustrezne imenske prostore (namespaces):
`BolnisnicaApp.Models`, `BolnisnicaApp.Interfaces`, `BolnisnicaApp.Extensions`
in `BolnisnicaApp` (glavni razred `Program`).

## Pregled rešitev po nalogah

**Naloga 1 - razredni model (15 točk)**
- `Zaposleni` je abstrakten, vsebuje abstraktno funkcijo `OpisDelovnihNalog()`.
- `Zdravnik` in `MedicinskaSestra` dedujeta od `Zaposleni`.
- `Pacient` ima osebne podatke (`Ime`, `Priimek`, `DatumRojstva`) in
  kartoteko pregledov (`KartotekaPregledov`).
- `Zdravnik.Pacienti` - seznam pacientov, ki jih zdravnik zdravi.
- `MedicinskaSestra.DodeljeniPacienti` - seznam pacientov, ki jih sestra neguje.
- `Bolnisnica.SeznamZaposlenih` in `Bolnisnica.Pacienti` - seznami zaposlenih in pacientov.
- V vsakem razredu je vsaj ena samodejno implementirana lastnost (auto-property)
  in vsaj ena z lastno implementacijo (backing field + lastna logika v `get`/`set`):
  - `Zaposleni.DatumZaposlitve` - preveri, da datum ni v prihodnosti.
  - `Zdravnik.TerminiPoDatumih` - glej Nalogo 2.
  - `MedicinskaSestra.Naloge` - zaseben `set`, polni se prek `IZadolzen`.
  - `Pacient.KartotekaPregledov` - poskrbi, da seznam ni `null`.
  - `Bolnisnica.SeznamZaposlenih` - dodajanje prek `DodajZaposlenega` (brez podvajanja).

**Naloga 2 - termini zdravnika po datumih (10 točk)**
- `Zdravnik.TerminiPoDatumih` je tipa `Dictionary<DateTime, List<Pacient>>`.
- Utemeljitev (komentar v `Zdravnik.cs`): slovar omogoča hitro (O(1)) iskanje
  pacientov za izbrani dan, ključ (datum) je naraven in enoličen, vrednost
  (seznam) pa omogoča več pacientov na isti dan.

**Naloga 3 - Direktor kot Singleton (10 točk)**
- `Direktor.Instance(...)` vrne edino instanco razreda (privaten konstruktor).
- `Razgovor(Zaposleni)` vrne naključno celo število med 1 in 5.

**Naloga 4 - glavni razred / Main (6 točk)**
- V `Program.Main` je ustvarjena `Bolnisnica`, 2 zdravnika, 2 medicinski
  sestri in 3 pacienti, vse lastnosti so napolnjene s podatki.

**Naloga 5 - LINQ izpis pacientov na izbrani dan (10 točk)**
- Statična funkcija `IzpisiPacienteNaDan(Bolnisnica, DateTime)` z uporabo
  LINQ (`OfType`, `Where`, `SelectMany`, `Distinct`) izpiše vse paciente,
  ki imajo na dani dan pregled pri katerem koli zdravniku.

**Naloga 6 - vmesnik IZadolzen (10 točk)**
- `IZadolzen` zahteva `PrevzemiNalogo`, `ZakljuciNalogo`, `PrenesiNalogo`.
- Implementiran v `MedicinskaSestra`. `PrevzemiNalogo` je implementiran
  eksplicitno (`void IZadolzen.PrevzemiNalogo(...)`), ostali dve funkciji
  pa implicitno (javni metodi razreda).

**Naloga 7 - povprečje pacientov na zdravnika (9 točk)**
- Razširitvena metoda `Bolnisnica.PovprecnoPacientovNaZdravnika()`
  v `Extensions/BolnisnicaExtensions.cs`, ki uporablja LINQ (`OfType<Zdravnik>().Average(...)`).
- Klicana je v `Program.Main`.

## Opomba

Koda v tem okolju ni bila prevedena (na strežniku ni nameščenega .NET SDK),
zato pred oddajo priporočam, da projekt odprete v Visual Studiu / VS Code,
ga zaženete (F5 / `dotnet run`) in preverite izpis v konzoli.
