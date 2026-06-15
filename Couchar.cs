using System;
using System.Collections.Generic;
using System.Linq;

namespace Couchar
{
    // =====================================================================
    // ENUMERACIJE
    // =====================================================================

    /// <summary>Vrsta/raven tekmovanja.</summary>
    public enum TipTekmovanja
    {
        Evropsko,
        Svetovno,
        Drzavno,
        Regionalno,
        Lokalno
    }

    /// <summary>Sistem izvedbe tekmovanja.</summary>
    public enum SistemTekmovanja
    {
        Ligaski,
        Pokalni,
        Skupinski
    }

    /// <summary>Tip ekipe - klub ali drzavna reprezentanca.</summary>
    public enum TipEkipe
    {
        Klub,
        DrzavnaReprezentanca
    }

    /// <summary>Podprte vrste sportov.</summary>
    public enum VrstaSporta
    {
        Nogomet,
        Kosarka
    }

    /// <summary>Tip kartona pri nogometu.</summary>
    public enum TipKartona
    {
        Rumeni,
        Rdeci
    }

    // =====================================================================
    // VMESNIKI (interfaces)
    // =====================================================================

    /// <summary>
    /// Vmesnik, ki ga implementira vsak sport. Definira, koliko tock
    /// (za potrebe vrstnega reda na tekmovanju) prinese posamezna
    /// kombinacija statistike "lastne" ekipe in statistike nasprotnika.
    /// S tem je metoda IzracunajVrstniRed v razredu Tekmovanje
    /// neodvisna od konkretnega sporta - sport sam pove, kako se tocke racunajo.
    /// </summary>
    public interface ITockovanje
    {
        int IzracunajTocke(Statistika lastnaStatistika, Statistika statistikaNasprotnika);
    }

    // =====================================================================
    // STATISTIKA (abstraktni razred + dedovanje)
    // =====================================================================

    /// <summary>
    /// Abstraktni razred, ki predstavlja statistiko ene ekipe na eni tekmi.
    /// Konkretne vsebine (goli, koshi, ...) so odvisne od sporta in jih
    /// definirajo podrazredi.
    /// </summary>
    public abstract class Statistika
    {
        // Readonly - tekma, na katero se statistika nanasa, je dolocena
        // ob kreiranju in se kasneje ne spremeni.
        public Tekma Tekma { get; }

        protected Statistika(Tekma tekma)
        {
            Tekma = tekma;
        }

        public abstract override string ToString();
    }

    /// <summary>Statistika ekipe pri nogometu.</summary>
    public class NogometnaStatistika : Statistika
    {
        public int Goli { get; set; }
        public int RumeniKartoni { get; set; }
        public int RdeciKartoni { get; set; }
        public double PosestZoge { get; set; } // v odstotkih [0-100]

        public NogometnaStatistika(Tekma tekma) : base(tekma) { }

        public override string ToString()
        {
            return $"Goli: {Goli}, Rumeni kartoni: {RumeniKartoni}, " +
                   $"Rdeci kartoni: {RdeciKartoni}, Posest zoge: {PosestZoge:F1}%";
        }
    }

    /// <summary>Statistika ekipe pri kosarki.</summary>
    public class KosarkarskaStatistika : Statistika
    {
        public int Koshi { get; set; }
        public int Asistence { get; set; }
        public int Skoki { get; set; }

        public KosarkarskaStatistika(Tekma tekma) : base(tekma) { }

        public override string ToString()
        {
            return $"Koshi: {Koshi}, Asistence: {Asistence}, Skoki: {Skoki}";
        }
    }

    /// <summary>
    /// Statistika posameznega igralca - poseben razred, ki ni neposredno
    /// del hierarhije Statistika (ni nujno vezan na en sport), ampak
    /// se navezuje na konkretnega igralca.
    /// </summary>
    public class StatistikaIgralca
    {
        // Readonly - igralec, na katerega se statistika nanasa.
        public Igralec Igralec { get; }

        public int OdigraneMinute { get; set; }
        public int Goli { get; set; }
        public int Asistence { get; set; }

        public StatistikaIgralca(Igralec igralec)
        {
            Igralec = igralec;
        }

        public override string ToString()
        {
            return $"{Igralec}: {OdigraneMinute} min, {Goli} golov, {Asistence} asistenc";
        }
    }

    // =====================================================================
    // SPORT (abstraktni razred + dedovanje + implementacija vmesnika)
    // =====================================================================

    /// <summary>
    /// Abstraktni razred, ki predstavlja sport. Vsak konkretni sport
    /// (Nogomet, Kosarka, ...) ve, kaksen tip statistike uporablja in
    /// kako iz statistike izracunati tocke za vrstni red (ITockovanje).
    /// </summary>
    public abstract class Sport : ITockovanje
    {
        // Readonly lastnosti - dolocene ob kreiranju konkretnega sporta.
        public string Ime { get; }
        public VrstaSporta Vrsta { get; }

        protected Sport(string ime, VrstaSporta vrsta)
        {
            Ime = ime;
            Vrsta = vrsta;
        }

        /// <summary>Tovarniska metoda - vsak sport ustvari "svojo" statistiko.</summary>
        public abstract Statistika UstvariStatistiko(Tekma tekma);

        /// <summary>Pretvorba statistike v tocke za vrstni red - odvisno od sporta.</summary>
        public abstract int IzracunajTocke(Statistika lastnaStatistika, Statistika statistikaNasprotnika);

        public override string ToString() => Ime;
    }

    public class Nogomet : Sport
    {
        public Nogomet() : base("Nogomet", VrstaSporta.Nogomet) { }

        public override Statistika UstvariStatistiko(Tekma tekma) => new NogometnaStatistika(tekma);

        public override int IzracunajTocke(Statistika lastna, Statistika nasprotnika)
        {
            var l = (NogometnaStatistika)lastna;
            var n = (NogometnaStatistika)nasprotnika;

            if (l.Goli > n.Goli) return 3; // zmaga
            if (l.Goli == n.Goli) return 1; // neodloceno
            return 0;                       // poraz
        }
    }

    public class Kosarka : Sport
    {
        public Kosarka() : base("Kosarka", VrstaSporta.Kosarka) { }

        public override Statistika UstvariStatistiko(Tekma tekma) => new KosarkarskaStatistika(tekma);

        public override int IzracunajTocke(Statistika lastna, Statistika nasprotnika)
        {
            var l = (KosarkarskaStatistika)lastna;
            var n = (KosarkarskaStatistika)nasprotnika;

            // V kosarki ni neodlocenih izidov.
            return l.Koshi > n.Koshi ? 2 : 1;
        }
    }

    // =====================================================================
    // EKIPE (abstraktni razred + dedovanje)
    // =====================================================================

    /// <summary>
    /// Abstraktni razred za ekipo. Konkretne ekipe so klub ali
    /// drzavna reprezentanca.
    /// </summary>
    public abstract class Ekipa
    {
        // Readonly - identifikacijski podatki ekipe se po kreiranju ne spremenijo.
        public string Ime { get; }
        public TipEkipe Tip { get; }

        public string Drzava { get; set; }

        // Readonly seznam - od zunaj je seznam igralcev viden samo za branje,
        // dodajanje pa je mozno le preko metode DodajIgralca.
        private readonly List<Igralec> igralci = new List<Igralec>();
        public IReadOnlyList<Igralec> Igralci => igralci;

        protected Ekipa(string ime, TipEkipe tip, string drzava)
        {
            Ime = ime;
            Tip = tip;
            Drzava = drzava;
        }

        public void DodajIgralca(Igralec igralec) => igralci.Add(igralec);

        public override string ToString() => $"{Ime} ({Drzava})";
    }

    public class Klub : Ekipa
    {
        public string Stadion { get; set; }
        public string Sponzor { get; set; }

        public Klub(string ime, string drzava, string stadion, string sponzor)
            : base(ime, TipEkipe.Klub, drzava)
        {
            Stadion = stadion;
            Sponzor = sponzor;
        }
    }

    public class DrzavnaReprezentanca : Ekipa
    {
        public string Selektor { get; set; }

        public DrzavnaReprezentanca(string ime, string drzava, string selektor)
            : base(ime, TipEkipe.DrzavnaReprezentanca, drzava)
        {
            Selektor = selektor;
        }
    }

    /// <summary>Posamezen igralec, ki nastopa za doloceno ekipo.</summary>
    public class Igralec
    {
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public string Pozicija { get; set; }

        // Readonly - igralec je "zaposlen" pri tocno doloceni ekipi,
        // ki je dolocena ob kreiranju.
        public Ekipa Ekipa { get; }

        public Igralec(string ime, string priimek, string pozicija, Ekipa ekipa)
        {
            Ime = ime;
            Priimek = priimek;
            Pozicija = pozicija;
            Ekipa = ekipa;
        }

        public override string ToString() => $"{Ime} {Priimek} ({Pozicija})";
    }

    // =====================================================================
    // TEKMA
    // =====================================================================

    public class Tekma
    {
        // Readonly - tekmovanje, na katerem je tekma odigrana, se ne spremeni.
        public Tekmovanje Tekmovanje { get; }

        // Readonly - ekipi, ki se pomerita, sta dolocene ob kreiranju tekme.
        public Ekipa Ekipa1 { get; }
        public Ekipa Ekipa2 { get; }

        public DateTime Datum { get; set; }

        // Statistiki obeh ekip - tip je odvisen od sporta tekmovanja
        // in se doloci avtomatsko ob kreiranju tekme.
        public Statistika Statistika1 { get; }
        public Statistika Statistika2 { get; }

        public Tekma(Tekmovanje tekmovanje, Ekipa ekipa1, Ekipa ekipa2, DateTime datum)
        {
            Tekmovanje = tekmovanje;
            Ekipa1 = ekipa1;
            Ekipa2 = ekipa2;
            Datum = datum;

            // Sport tekmovanja (preko Factory metode UstvariStatistiko)
            // dolocata, kaksen tip Statistika dobi vsaka ekipa.
            Statistika1 = tekmovanje.Sport.UstvariStatistiko(this);
            Statistika2 = tekmovanje.Sport.UstvariStatistiko(this);
        }

        public override string ToString() => $"{Ekipa1} - {Ekipa2} ({Datum:d})";
    }

    // =====================================================================
    // TEKMOVANJE
    // =====================================================================

    public class Tekmovanje
    {
        // Readonly - ime tekmovanja se po kreiranju ne spremeni.
        public string Ime { get; }

        public TipTekmovanja Tip { get; set; }
        public SistemTekmovanja Sistem { get; set; }

        // Readonly - sport tekmovanja je dolocen ob kreiranju in se ne menja
        // (tekme in statistike so odvisne od njega).
        public Sport Sport { get; }

        private readonly List<Ekipa> ekipe = new List<Ekipa>();
        public IReadOnlyList<Ekipa> Ekipe => ekipe;

        private readonly List<Tekma> tekme = new List<Tekma>();
        public IReadOnlyList<Tekma> Tekme => tekme;

        public Tekmovanje(string ime, TipTekmovanja tip, SistemTekmovanja sistem, Sport sport)
        {
            Ime = ime;
            Tip = tip;
            Sistem = sistem;
            Sport = sport;
        }

        public void DodajEkipo(Ekipa ekipa) => ekipe.Add(ekipa);

        public void DodajTekmo(Tekma tekma) => tekme.Add(tekma);

        /// <summary>
        /// (4) Izracuna trenutni vrstni red ekip na tekmovanju.
        /// Funkcija je neodvisna od konkretnega sporta, ker se za
        /// pretvorbo statistike v tocke uporabi Sport.IzracunajTocke
        /// (vmesnik ITockovanje) - vsak sport sam definira svoja pravila.
        /// </summary>
        public List<Ekipa> IzracunajVrstniRed()
        {
            var tockeEkip = ekipe.ToDictionary(e => e, e => 0);

            foreach (var tekma in tekme)
            {
                int tocke1 = Sport.IzracunajTocke(tekma.Statistika1, tekma.Statistika2);
                int tocke2 = Sport.IzracunajTocke(tekma.Statistika2, tekma.Statistika1);

                if (tockeEkip.ContainsKey(tekma.Ekipa1))
                    tockeEkip[tekma.Ekipa1] += tocke1;

                if (tockeEkip.ContainsKey(tekma.Ekipa2))
                    tockeEkip[tekma.Ekipa2] += tocke2;
            }

            return tockeEkip
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => kv.Key.Ime)
                .Select(kv => kv.Key)
                .ToList();
        }

        /// <summary>
        /// (7) Vrne seznam vseh tekem, v katerih je nastopila dana ekipa.
        /// Uporablja LINQ (Where) in lambda izraz.
        /// </summary>
        public List<Tekma> TekmeEkipe(Ekipa ekipa)
        {
            return tekme
                .Where(t => t.Ekipa1 == ekipa || t.Ekipa2 == ekipa)
                .ToList();
        }

        public override string ToString() => $"{Ime} ({Tip}, {Sport})";
    }

    // =====================================================================
    // FACTORY NACRTOVALSKI VZOREC
    // =====================================================================

    /// <summary>Abstraktna fabrika sportov.</summary>
    public abstract class SportFactory
    {
        public abstract Sport UstvariSport();
    }

    public class NogometFactory : SportFactory
    {
        public override Sport UstvariSport() => new Nogomet();
    }

    public class KosarkaFactory : SportFactory
    {
        public override Sport UstvariSport() => new Kosarka();
    }

    /// <summary>Pomozni razred, ki glede na izbiro uporabnika vrne ustrezno fabriko.</summary>
    public static class SportFactoryPonudnik
    {
        public static SportFactory PridobiFactory(VrstaSporta vrsta)
        {
            return vrsta switch
            {
                VrstaSporta.Nogomet => new NogometFactory(),
                VrstaSporta.Kosarka => new KosarkaFactory(),
                _ => throw new ArgumentException("Neznana vrsta sporta.")
            };
        }
    }

    /// <summary>
    /// (3) Razred uporabniskega vmesnika, ki s pomocjo Factory vzorca
    /// uporabniku omogoca vnos novega tekmovanja in ekip, ki v njem nastopajo.
    /// Factory vzorec se uporabi za kreiranje pravega podrazreda Sport
    /// (Nogomet/Kosarka) glede na izbiro uporabnika, brez da bi UI
    /// poznal konkretne razrede sportov.
    /// </summary>
    public class UporabniskiVmesnik
    {
        public Tekmovanje VnesiTekmovanje()
        {
            Console.WriteLine("=== Vnos novega tekmovanja ===");

            Console.Write("Ime tekmovanja: ");
            string ime = Console.ReadLine();

            Console.Write("Vrsta sporta (0 - Nogomet, 1 - Kosarka): ");
            VrstaSporta vrstaSporta = (VrstaSporta)int.Parse(Console.ReadLine());

            Console.Write("Tip tekmovanja (0-Evropsko, 1-Svetovno, 2-Drzavno, 3-Regionalno, 4-Lokalno): ");
            TipTekmovanja tip = (TipTekmovanja)int.Parse(Console.ReadLine());

            Console.Write("Sistem tekmovanja (0-Ligaski, 1-Pokalni, 2-Skupinski): ");
            SistemTekmovanja sistem = (SistemTekmovanja)int.Parse(Console.ReadLine());

            // --- uporaba Factory vzorca ---
            SportFactory factory = SportFactoryPonudnik.PridobiFactory(vrstaSporta);
            Sport sport = factory.UstvariSport();
            // --------------------------------

            var tekmovanje = new Tekmovanje(ime, tip, sistem, sport);

            Console.Write("Stevilo ekip, ki jih zelite vnesti: ");
            int steviloEkip = int.Parse(Console.ReadLine());

            for (int i = 0; i < steviloEkip; i++)
            {
                Console.WriteLine($"--- Ekipa {i + 1} ---");

                Console.Write("Ime ekipe: ");
                string imeEkipe = Console.ReadLine();

                Console.Write("Drzava: ");
                string drzava = Console.ReadLine();

                Console.Write("Tip ekipe (0-Klub, 1-DrzavnaReprezentanca): ");
                TipEkipe tipEkipe = (TipEkipe)int.Parse(Console.ReadLine());

                Ekipa ekipa;
                if (tipEkipe == TipEkipe.Klub)
                {
                    Console.Write("Stadion: ");
                    string stadion = Console.ReadLine();

                    Console.Write("Sponzor: ");
                    string sponzor = Console.ReadLine();

                    ekipa = new Klub(imeEkipe, drzava, stadion, sponzor);
                }
                else
                {
                    Console.Write("Selektor: ");
                    string selektor = Console.ReadLine();

                    ekipa = new DrzavnaReprezentanca(imeEkipe, drzava, selektor);
                }

                tekmovanje.DodajEkipo(ekipa);
            }

            return tekmovanje;
        }
    }

    // =====================================================================
    // RAZSIRITVENA METODA (6)
    // =====================================================================

    /// <summary>
    /// Razred z razsiritveno metodo za Ekipa. Metoda IzpisiStatistiko
    /// zdruzi ToString ekipe in ToString podane statistike.
    /// </summary>
    public static class EkipaRazsiritve
    {
        public static string IzpisiStatistiko(this Ekipa ekipa, Statistika statistika)
        {
            return ekipa.ToString() + " - " + statistika.ToString();
        }
    }

    // =====================================================================
    // PRIMER UPORABE (5)
    // =====================================================================

    public class Program
    {
        public static void Main()
        {
            // --- Kreiranje sporta in tekmovanja ---
            Sport nogomet = new Nogomet();
            var em = new Tekmovanje(
                "Evropsko prvenstvo 2024",
                TipTekmovanja.Evropsko,
                SistemTekmovanja.Skupinski,
                nogomet);

            // --- Kreiranje ekip ---
            var slovenija = new DrzavnaReprezentanca("Slovenija", "Slovenija", "Matjaz Kek");
            var anglija = new DrzavnaReprezentanca("Anglija", "Anglija", "Gareth Southgate");
            var danska = new DrzavnaReprezentanca("Danska", "Danska", "Kasper Hjulmand");

            em.DodajEkipo(slovenija);
            em.DodajEkipo(anglija);
            em.DodajEkipo(danska);

            // --- Rocno kreiranje nekaj tekem in vnos statistike ---
            var tekma1 = new Tekma(em, slovenija, danska, new DateTime(2024, 6, 16));
            ((NogometnaStatistika)tekma1.Statistika1).Goli = 1;
            ((NogometnaStatistika)tekma1.Statistika2).Goli = 1;
            em.DodajTekmo(tekma1);

            var tekma2 = new Tekma(em, anglija, slovenija, new DateTime(2024, 6, 25));
            ((NogometnaStatistika)tekma2.Statistika1).Goli = 0;
            ((NogometnaStatistika)tekma2.Statistika2).Goli = 0;
            em.DodajTekmo(tekma2);

            var tekma3 = new Tekma(em, danska, anglija, new DateTime(2024, 6, 20));
            ((NogometnaStatistika)tekma3.Statistika1).Goli = 1;
            ((NogometnaStatistika)tekma3.Statistika2).Goli = 1;
            em.DodajTekmo(tekma3);

            // --- (5) Izpis trenutnega vrstnega reda ---
            Console.WriteLine("Trenutni vrstni red:");
            var vrstniRed = em.IzracunajVrstniRed();
            for (int i = 0; i < vrstniRed.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {vrstniRed[i]}");
            }

            // --- (6) Primer uporabe razsiritvene metode IzpisiStatistiko ---
            Console.WriteLine();
            Console.WriteLine(slovenija.IzpisiStatistiko(tekma1.Statistika1));

            // --- (7) Primer uporabe TekmeEkipe ---
            Console.WriteLine();
            Console.WriteLine("Tekme Slovenije:");
            foreach (var t in em.TekmeEkipe(slovenija))
            {
                Console.WriteLine(t);
            }
        }
    }
}
