using System;
using System.Collections.Generic;
using System.Linq;

namespace BolnisnicaApp
{
    #region NALOGA 1 - Razredni model

    /// <summary>
    /// Abstraktni razred, ki predstavlja zaposlenega v bolnišnici.
    /// Razreda Zdravnik in MedicinskaSestra sta njegova podrazreda.
    /// </summary>
    public abstract class Zaposleni
    {
        // --- Samodejno implementirani lastnosti (auto-implemented properties) ---
        public string Ime { get; set; }
        public string Priimek { get; set; }

        // --- Ne-samodejno implementirana lastnost ---
        // Datum zaposlitve ima lastno polje (backing field) in lastno logiko
        // v "set" delu, ki preveri, da datum zaposlitve ni v prihodnosti.
        private DateTime datumZaposlitve;
        public DateTime DatumZaposlitve
        {
            get => datumZaposlitve;
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("Datum zaposlitve ne more biti v prihodnosti.");
                }
                datumZaposlitve = value;
            }
        }

        protected Zaposleni(string ime, string priimek, DateTime datumZaposlitve)
        {
            Ime = ime;
            Priimek = priimek;
            DatumZaposlitve = datumZaposlitve;
        }

        /// <summary>
        /// Abstraktna funkcija, ki jo morajo implementirati vsi podrazredi
        /// in vrne kratek opis delovnih nalog zaposlenega.
        /// </summary>
        public abstract string OpisDelovnihNalog();

        public override string ToString()
        {
            return $"{Ime} {Priimek}";
        }
    }

    /// <summary>
    /// Razred predstavlja pacienta - hrani osebne podatke ter kartoteko pregledov.
    /// </summary>
    public class Pacient
    {
        // --- Samodejno implementirane lastnosti ---
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public DateTime DatumRojstva { get; set; }

        // --- Ne-samodejno implementirana lastnost ---
        // Kartoteka pregledov ima lastno polje. "Set" del poskrbi, da
        // seznam ni nikoli null (privzeto vrne prazen seznam).
        private List<string> kartotekaPregledov = new List<string>();
        public List<string> KartotekaPregledov
        {
            get => kartotekaPregledov;
            set => kartotekaPregledov = value ?? new List<string>();
        }

        public Pacient(string ime, string priimek, DateTime datumRojstva)
        {
            Ime = ime;
            Priimek = priimek;
            DatumRojstva = datumRojstva;
        }

        /// <summary>
        /// Doda nov vnos v kartoteko pregledov pacienta.
        /// </summary>
        public void DodajPregled(string opisPregleda)
        {
            kartotekaPregledov.Add($"{DateTime.Now:dd.MM.yyyy HH:mm} - {opisPregleda}");
        }

        public override string ToString()
        {
            return $"{Ime} {Priimek}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Pacient drugi)
            {
                return Ime == drugi.Ime && Priimek == drugi.Priimek && DatumRojstva == drugi.DatumRojstva;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ime, Priimek, DatumRojstva);
        }
    }

    #endregion

    #region NALOGA 2 - Zdravnik (vsebuje tudi termine po datumih)

    /// <summary>
    /// Razred predstavlja zdravnika, podrazred abstraktnega razreda Zaposleni.
    /// </summary>
    public class Zdravnik : Zaposleni
    {
        // --- Samodejno implementirani lastnosti ---
        public string Specializacija { get; set; }

        // Seznam pacientov, ki jih zdravnik zdravi.
        public List<Pacient> Pacienti { get; set; } = new List<Pacient>();

        // --- Ne-samodejno implementirana lastnost (Naloga 2) ---
        //
        // Lastnost TerminiPoDatumih za vsak datum hrani seznam pacientov,
        // ki so tisti dan naročeni na pregled pri tem zdravniku.
        //
        // Utemeljitev izbire podatkovne strukture:
        // Uporabljen je Dictionary<DateTime, List<Pacient>>, ker:
        //   - ključ (DateTime - dan termina) je naraven, enoličen identifikator,
        //     zato je iskanje vseh pacientov za določen dan v povprečju O(1),
        //     kar je hitreje kot npr. iskanje po seznamu (List) z O(n);
        //   - vrednost je List<Pacient>, ker je na isti dan lahko naročenih
        //     več pacientov, vrstni red naročanja pa je smiseln (kdo je bil
        //     naročen prej);
        //   - Dictionary omogoča enostavno preverjanje, ali za izbrani dan
        //     sploh obstajajo kakšni termini (ContainsKey), brez potrebe po
        //     iskanju po celotni zgodovini terminov.
        private Dictionary<DateTime, List<Pacient>> terminiPoDatumih = new Dictionary<DateTime, List<Pacient>>();

        public Dictionary<DateTime, List<Pacient>> TerminiPoDatumih
        {
            get => terminiPoDatumih;
            private set => terminiPoDatumih = value ?? new Dictionary<DateTime, List<Pacient>>();
        }

        public Zdravnik(string ime, string priimek, DateTime datumZaposlitve, string specializacija)
            : base(ime, priimek, datumZaposlitve)
        {
            Specializacija = specializacija;
        }

        /// <summary>
        /// Naroči pacienta na pregled na izbrani datum.
        /// </summary>
        public void NarociPacienta(DateTime datum, Pacient pacient)
        {
            DateTime dan = datum.Date;

            if (!terminiPoDatumih.ContainsKey(dan))
            {
                terminiPoDatumih[dan] = new List<Pacient>();
            }

            terminiPoDatumih[dan].Add(pacient);
        }

        public override string OpisDelovnihNalog()
        {
            return $"Zdravnik {Ime} {Priimek} (specializacija: {Specializacija}) " +
                   $"skrbi za {Pacienti.Count} pacientov in opravlja preglede po dogovorjenih terminih.";
        }
    }

    #endregion

    #region NALOGA 6 - Vmesnik IZadolzen in pomožni razred Naloga

    /// <summary>
    /// Vmesnik, ki ga implementirajo zaposleni, ki so lahko zadolženi za naloge.
    /// </summary>
    public interface IZadolzen
    {
        /// <summary>
        /// Zaposleni prevzame novo nalogo z danim opisom.
        /// </summary>
        void PrevzemiNalogo(string opis);

        /// <summary>
        /// Označi nalogo z danim ID-jem kot zaključeno.
        /// </summary>
        void ZakljuciNalogo(int idNaloge);

        /// <summary>
        /// Prenese nalogo z danim ID-jem na drugega zaposlenega.
        /// </summary>
        void PrenesiNalogo(int idNaloge, Zaposleni novZadolzeni);
    }

    /// <summary>
    /// Pomožni razred, ki predstavlja eno delovno nalogo.
    /// Uporablja se pri implementaciji vmesnika IZadolzen.
    /// </summary>
    public class Naloga
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public Zaposleni Zadolzeni { get; set; }
        public bool Zakljucena { get; set; }

        public Naloga(int id, string opis, Zaposleni zadolzeni)
        {
            Id = id;
            Opis = opis;
            Zadolzeni = zadolzeni;
            Zakljucena = false;
        }

        public override string ToString()
        {
            string status = Zakljucena ? "zaključena" : "v izvajanju";
            return $"#{Id}: {Opis} (zadolžen: {Zadolzeni}, status: {status})";
        }
    }

    #endregion

    #region NALOGA 1 in 6 - MedicinskaSestra

    /// <summary>
    /// Razred predstavlja medicinsko sestro, podrazred abstraktnega razreda Zaposleni.
    /// Implementira vmesnik IZadolzen (Naloga 6).
    /// </summary>
    public class MedicinskaSestra : Zaposleni, IZadolzen
    {
        // --- Samodejno implementirani lastnosti ---
        public string Oddelek { get; set; }

        // Seznam pacientov, ki jih medicinska sestra neguje.
        public List<Pacient> DodeljeniPacienti { get; set; } = new List<Pacient>();

        // --- Ne-samodejno implementirana lastnost ---
        // Seznam nalog ima zaseben "set", ker se naloge dodajajo izključno
        // prek metod vmesnika IZadolzen (PrevzemiNalogo/PrenesiNalogo).
        private List<Naloga> naloge = new List<Naloga>();
        public List<Naloga> Naloge
        {
            get => naloge;
            private set => naloge = value ?? new List<Naloga>();
        }

        public MedicinskaSestra(string ime, string priimek, DateTime datumZaposlitve, string oddelek)
            : base(ime, priimek, datumZaposlitve)
        {
            Oddelek = oddelek;
        }

        public override string OpisDelovnihNalog()
        {
            return $"Medicinska sestra {Ime} {Priimek} (oddelek: {Oddelek}) " +
                   $"neguje {DodeljeniPacienti.Count} pacientov in ima {naloge.Count} dodeljenih nalog.";
        }

        // --- Implementacija vmesnika IZadolzen ---

        // Ta funkcija je implementirana EKSPLICITNO (Naloga 6 zahteva, da je
        // vsaj ena funkcija implementirana eksplicitno). Dostopna je le prek
        // referenc tipa IZadolzen, npr.: ((IZadolzen)sestra).PrevzemiNalogo(...)
        void IZadolzen.PrevzemiNalogo(string opis)
        {
            int novId = naloge.Count == 0 ? 1 : naloge.Max(n => n.Id) + 1;
            naloge.Add(new Naloga(novId, opis, this));
            Console.WriteLine($"{Ime} {Priimek} je prevzela nalogo #{novId}: \"{opis}\".");
        }

        public void ZakljuciNalogo(int idNaloge)
        {
            Naloga naloga = naloge.FirstOrDefault(n => n.Id == idNaloge);

            if (naloga != null)
            {
                naloga.Zakljucena = true;
                Console.WriteLine($"Naloga #{idNaloge} je bila zaključena s strani {Ime} {Priimek}.");
            }
            else
            {
                Console.WriteLine($"Naloga #{idNaloge} ne obstaja pri {Ime} {Priimek}.");
            }
        }

        public void PrenesiNalogo(int idNaloge, Zaposleni novZadolzeni)
        {
            Naloga naloga = naloge.FirstOrDefault(n => n.Id == idNaloge);

            if (naloga == null)
            {
                Console.WriteLine($"Naloga #{idNaloge} ne obstaja pri {Ime} {Priimek}.");
                return;
            }

            naloga.Zadolzeni = novZadolzeni;
            naloge.Remove(naloga);

            // Če je nov zadolženec tudi medicinska sestra, naloga preide v njen seznam.
            if (novZadolzeni is MedicinskaSestra novaSestra)
            {
                novaSestra.naloge.Add(naloga);
            }

            Console.WriteLine($"Naloga #{idNaloge} je bila prenesena z {Ime} {Priimek} na {novZadolzeni}.");
        }
    }

    #endregion

    #region NALOGA 3 - Direktor (Singleton)

    /// <summary>
    /// Razred predstavlja direktorja bolnišnice. V sistemu lahko obstaja
    /// samo ena instanca tega razreda, zato je uporabljen oblikovalski
    /// vzorec Singleton (Naloga 3).
    /// </summary>
    public class Direktor : Zaposleni
    {
        // Edina (statična) instanca razreda Direktor.
        private static Direktor instance;

        // Generator naključnih ocen pri razgovorih.
        private static readonly Random random = new Random();

        // Privaten konstruktor preprečuje ustvarjanje instanc od zunaj,
        // s čimer je zagotovljeno, da je v sistemu lahko ustvarjena
        // samo ena instanca razreda Direktor.
        private Direktor(string ime, string priimek, DateTime datumZaposlitve)
            : base(ime, priimek, datumZaposlitve)
        {
        }

        /// <summary>
        /// Vrne edino instanco razreda Direktor. Ob prvem klicu jo ustvari
        /// s podanimi podatki, vsi nadaljnji klici pa vrnejo isto instanco
        /// (parametri se pri naslednjih klicih ignorirajo).
        /// </summary>
        public static Direktor Instance(string ime = "Igor", string priimek = "Sinkovec", DateTime? datumZaposlitve = null)
        {
            if (instance == null)
            {
                instance = new Direktor(ime, priimek, datumZaposlitve ?? DateTime.Now);
            }

            return instance;
        }

        /// <summary>
        /// Opravi razgovor z izbranim zaposlenim in vrne oceno zaposlenega
        /// po razgovoru - celo število med 1 in 5.
        /// </summary>
        public int Razgovor(Zaposleni zaposleni)
        {
            int ocena = random.Next(1, 6); // Next(1, 6) vrne vrednosti 1-5
            Console.WriteLine($"Direktor {Ime} {Priimek} je opravil razgovor z {zaposleni} - ocena: {ocena}/5.");
            return ocena;
        }

        public override string OpisDelovnihNalog()
        {
            return $"Direktor {Ime} {Priimek} vodi bolnišnico in vsako leto opravi razgovore z vsemi zaposlenimi.";
        }
    }

    #endregion

    #region NALOGA 1 - Bolnisnica

    /// <summary>
    /// Razred predstavlja bolnišnico - hrani seznam zaposlenih in seznam pacientov.
    /// </summary>
    public class Bolnisnica
    {
        // --- Samodejno implementirani lastnosti ---
        public string Naziv { get; set; }
        public List<Pacient> Pacienti { get; set; } = new List<Pacient>();

        // --- Ne-samodejno implementirana lastnost ---
        // Seznam zaposlenih ima zaseben backing field. "Set" del poskrbi,
        // da seznam ni nikoli null, dodajanje pa poteka prek metode
        // DodajZaposlenega, ki preprečuje podvajanje istega zaposlenega.
        private List<Zaposleni> seznamZaposlenih = new List<Zaposleni>();
        public List<Zaposleni> SeznamZaposlenih
        {
            get => seznamZaposlenih;
            set => seznamZaposlenih = value ?? new List<Zaposleni>();
        }

        public Bolnisnica(string naziv)
        {
            Naziv = naziv;
        }

        /// <summary>
        /// Doda zaposlenega v bolnišnico, če ta še ni v seznamu.
        /// </summary>
        public void DodajZaposlenega(Zaposleni zaposleni)
        {
            if (!seznamZaposlenih.Contains(zaposleni))
            {
                seznamZaposlenih.Add(zaposleni);
            }
        }

        public override string ToString()
        {
            return Naziv;
        }
    }

    #endregion

    #region NALOGA 7 - Razširitvena metoda

    /// <summary>
    /// Razširitveni razred za Bolnisnica (Naloga 7).
    /// </summary>
    public static class BolnisnicaExtensions
    {
        /// <summary>
        /// Razširitvena funkcija, ki vrne povprečno število pacientov
        /// na zdravnika v dani bolnišnici. Če v bolnišnici ni zdravnikov,
        /// vrne 0.
        /// </summary>
        public static double PovprecnoPacientovNaZdravnika(this Bolnisnica bolnisnica)
        {
            var zdravniki = bolnisnica.SeznamZaposlenih.OfType<Zdravnik>().ToList();

            if (zdravniki.Count == 0)
            {
                return 0;
            }

            return zdravniki.Average(z => z.Pacienti.Count);
        }
    }

    #endregion

    #region NALOGA 4 in 5 - Glavni razred (Program)

    /// <summary>
    /// Glavni razred sistema - vstopna točka aplikacije (Naloga 4).
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // ----------------------------------------------------------
            // NALOGA 4: Ustvarimo bolnišnico, vsaj 2 zdravnika, vsaj 2
            // medicinski sestri in vsaj 3 paciente, ter napolnimo
            // njihove lastnosti.
            // ----------------------------------------------------------
            Bolnisnica bolnisnica = new Bolnisnica("Splošna bolnišnica Novo mesto");

            Zdravnik zdravnik1 = new Zdravnik("Ana", "Kovač", new DateTime(2015, 3, 1), "Kardiologija");
            Zdravnik zdravnik2 = new Zdravnik("Marko", "Zupan", new DateTime(2018, 9, 15), "Kirurgija");

            MedicinskaSestra sestra1 = new MedicinskaSestra("Nina", "Horvat", new DateTime(2019, 5, 10), "Kardiologija");
            MedicinskaSestra sestra2 = new MedicinskaSestra("Tjaša", "Krajnc", new DateTime(2020, 1, 20), "Kirurgija");

            Pacient pacient1 = new Pacient("Janez", "Novak", new DateTime(1980, 4, 12));
            Pacient pacient2 = new Pacient("Maja", "Potočnik", new DateTime(1992, 7, 23));
            Pacient pacient3 = new Pacient("Peter", "Kralj", new DateTime(1975, 11, 5));

            // Zdravniki dobijo paciente, ki jih zdravijo.
            zdravnik1.Pacienti.Add(pacient1);
            zdravnik1.Pacienti.Add(pacient2);
            zdravnik2.Pacienti.Add(pacient3);
            zdravnik2.Pacienti.Add(pacient1);

            // Medicinske sestre dobijo dodeljene paciente, ki jih negujejo.
            sestra1.DodeljeniPacienti.Add(pacient1);
            sestra1.DodeljeniPacienti.Add(pacient2);
            sestra2.DodeljeniPacienti.Add(pacient3);

            // Dodamo nekaj zapisov v kartoteko pregledov pacientov.
            pacient1.DodajPregled("Pregled srca - vse v redu.");
            pacient2.DodajPregled("Splošni pregled.");
            pacient3.DodajPregled("Pooperativni pregled.");

            // ----------------------------------------------------------
            // NALOGA 2: Naročimo paciente na termine za določene datume.
            // ----------------------------------------------------------
            DateTime danes = DateTime.Today;
            DateTime jutri = danes.AddDays(1);

            zdravnik1.NarociPacienta(danes, pacient1);
            zdravnik1.NarociPacienta(danes, pacient2);
            zdravnik2.NarociPacienta(danes, pacient3);
            zdravnik2.NarociPacienta(jutri, pacient1);

            // Bolnišnica dobi seznam zaposlenih in seznam pacientov.
            bolnisnica.DodajZaposlenega(zdravnik1);
            bolnisnica.DodajZaposlenega(zdravnik2);
            bolnisnica.DodajZaposlenega(sestra1);
            bolnisnica.DodajZaposlenega(sestra2);

            bolnisnica.Pacienti.Add(pacient1);
            bolnisnica.Pacienti.Add(pacient2);
            bolnisnica.Pacienti.Add(pacient3);

            Console.WriteLine("=== Opisi delovnih nalog zaposlenih ===");
            foreach (Zaposleni z in bolnisnica.SeznamZaposlenih)
            {
                Console.WriteLine(z.OpisDelovnihNalog());
            }

            // ----------------------------------------------------------
            // NALOGA 3: Direktor je Singleton - obstaja samo ena instanca.
            // ----------------------------------------------------------
            Console.WriteLine("\n=== Razgovori z direktorjem ===");
            Direktor direktor = Direktor.Instance("Igor", "Sinkovec", new DateTime(2010, 1, 1));
            direktor.Razgovor(zdravnik1);
            direktor.Razgovor(sestra2);

            // Preverimo, da gre vedno za isto instanco.
            Direktor direktor2 = Direktor.Instance();
            Console.WriteLine($"Gre za isto instanco direktorja: {ReferenceEquals(direktor, direktor2)}");

            // ----------------------------------------------------------
            // NALOGA 5: Izpis vseh pacientov, ki imajo na dani datum
            // pregled pri katerem koli zdravniku (z uporabo LINQ).
            // ----------------------------------------------------------
            Console.WriteLine();
            IzpisiPacienteNaDan(bolnisnica, danes);
            IzpisiPacienteNaDan(bolnisnica, jutri);

            // ----------------------------------------------------------
            // NALOGA 6: Uporaba vmesnika IZadolzen.
            // ----------------------------------------------------------
            Console.WriteLine("\n=== Delovne naloge medicinske sestre ===");

            // PrevzemiNalogo je eksplicitna implementacija, zato jo kličemo
            // preko spremenljivke/referenece tipa IZadolzen.
            IZadolzen zadolzenaSestra = sestra1;
            zadolzenaSestra.PrevzemiNalogo("Priprava sobe za novega pacienta");
            zadolzenaSestra.PrevzemiNalogo("Razdelitev jutranjih zdravil");

            sestra1.ZakljuciNalogo(1);
            sestra1.PrenesiNalogo(2, sestra2);

            // ----------------------------------------------------------
            // NALOGA 7: Razširitvena funkcija - povprečno število
            // pacientov na zdravnika v bolnišnici.
            // ----------------------------------------------------------
            Console.WriteLine("\n=== Statistika ===");
            double povprecje = bolnisnica.PovprecnoPacientovNaZdravnika();
            Console.WriteLine($"Povprečno število pacientov na zdravnika v bolnišnici '{bolnisnica.Naziv}': {povprecje:F2}");
        }

        /// <summary>
        /// NALOGA 5: Statična funkcija, ki za dani datum in bolnišnico
        /// z uporabo LINQ izpiše seznam vseh pacientov, ki imajo na ta
        /// dan pregled pri katerem koli zdravniku.
        /// </summary>
        static void IzpisiPacienteNaDan(Bolnisnica bolnisnica, DateTime datum)
        {
            var pacientiNaDan = bolnisnica.SeznamZaposlenih
                .OfType<Zdravnik>()
                .Where(z => z.TerminiPoDatumih.ContainsKey(datum.Date))
                .SelectMany(z => z.TerminiPoDatumih[datum.Date])
                .Distinct()
                .ToList();

            Console.WriteLine($"Pacienti z pregledom na dan {datum:dd.MM.yyyy}:");

            if (pacientiNaDan.Count == 0)
            {
                Console.WriteLine("  (ni naročenih pacientov)");
            }
            else
            {
                foreach (Pacient p in pacientiNaDan)
                {
                    Console.WriteLine($"  - {p}");
                }
            }
        }
    }

    #endregion
}
