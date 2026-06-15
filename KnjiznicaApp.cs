using System;
using System.Collections.Generic;
using System.Linq;

namespace KnjiznicaApp
{
    // =====================================================================
    // ENUMERACIJE
    // =====================================================================

    // Splošen tip gradiva - uporabljen v metodi IzpisiGradiva in v Factory-ju
    public enum TipGradiva
    {
        Knjiga,
        PeriodicnaPublikacija,
        AvdioVizualnoGradivo
    }

    // Specifične "podvrste" znotraj posameznih tipov gradiva
    public enum VrstaKnjige
    {
        Roman,
        Leksikon,
        Slikanica
    }

    public enum VrstaPublikacije
    {
        Casopis,
        Revija
    }

    public enum VrstaAVGradiva
    {
        Vinilka,
        DVD
    }

    // =====================================================================
    // VMESNIK
    // =====================================================================

    // Vmesnik določa lastnosti/vedenje vseh objektov, ki jih je mogoče
    // izposoditi oziroma rezervirati. Implementira ga abstraktni razred
    // Gradivo, lahko pa bi ga implementiral tudi povsem drugačen razred,
    // ki nima nič skupnega z gradivom (npr. razred Kanu za izposojo
    // kanuja ob Soči).
    public interface IIzposojljivo
    {
        bool JeRezervirano { get; set; }
        bool JeIzposojeno { get; set; }

        void Rezerviraj();
        void Izposodi();
        void Vrni();
    }

    // =====================================================================
    // ABSTRAKTNI NADRAZRED
    // =====================================================================

    public abstract class Gradivo : IIzposojljivo
    {
        // Skupne lastnosti vseh gradiv
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Avtor { get; set; }
        public int LetoIzdaje { get; set; }

        public bool JeRezervirano { get; set; }
        public bool JeIzposojeno { get; set; }

        // Vsak podrazred pove, kateremu tipu (enumeraciji) pripada -
        // uporabljeno v Knjiznica.IzpisiGradiva
        public abstract TipGradiva Tip { get; }

        protected Gradivo(int id, string naslov, string avtor, int letoIzdaje)
        {
            Id = id;
            Naslov = naslov;
            Avtor = avtor;
            LetoIzdaje = letoIzdaje;
            JeRezervirano = false;
            JeIzposojeno = false;
        }

        public virtual void Rezerviraj()
        {
            if (JeIzposojeno)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' je že izposojeno in ga ni mogoče rezervirati.");
            if (JeRezervirano)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' je že rezervirano.");

            JeRezervirano = true;
        }

        public virtual void Izposodi()
        {
            if (JeIzposojeno)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' je že izposojeno.");

            JeIzposojeno = true;
            JeRezervirano = false; // z izposojo morebitna rezervacija ugasne
        }

        public virtual void Vrni()
        {
            if (!JeIzposojeno)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' ni izposojeno, zato ga ni mogoče vrniti.");

            JeIzposojeno = false;
        }

        public override string ToString()
        {
            string stanje = JeIzposojeno ? "izposojeno"
                          : JeRezervirano ? "rezervirano"
                          : "na voljo";

            return $"[{Id}] {Naslov} ({Avtor}, {LetoIzdaje}) - {Tip} - {stanje}";
        }
    }

    // =====================================================================
    // PODRAZREDI - KONKRETNE VRSTE GRADIV
    // =====================================================================

    public class Knjiga : Gradivo
    {
        public VrstaKnjige Vrsta { get; set; }   // specifična lastnost 1
        public int SteviloStrani { get; set; }   // specifična lastnost 2

        public Knjiga(int id, string naslov, string avtor, int letoIzdaje,
                       VrstaKnjige vrsta, int steviloStrani)
            : base(id, naslov, avtor, letoIzdaje)
        {
            Vrsta = vrsta;
            SteviloStrani = steviloStrani;
        }

        public override TipGradiva Tip => TipGradiva.Knjiga;

        public override string ToString()
            => base.ToString() + $" | Vrsta knjige: {Vrsta}, Strani: {SteviloStrani}";
    }

    public class PeriodicnaPublikacija : Gradivo
    {
        public VrstaPublikacije Vrsta { get; set; }   // specifična lastnost 1
        public int StevilkaIzdaje { get; set; }       // specifična lastnost 2

        public PeriodicnaPublikacija(int id, string naslov, string avtor, int letoIzdaje,
                                       VrstaPublikacije vrsta, int stevilkaIzdaje)
            : base(id, naslov, avtor, letoIzdaje)
        {
            Vrsta = vrsta;
            StevilkaIzdaje = stevilkaIzdaje;
        }

        public override TipGradiva Tip => TipGradiva.PeriodicnaPublikacija;

        public override string ToString()
            => base.ToString() + $" | Vrsta publikacije: {Vrsta}, Številka izdaje: {StevilkaIzdaje}";
    }

    public class AvdioVizualnoGradivo : Gradivo
    {
        public VrstaAVGradiva Vrsta { get; set; }   // specifična lastnost 1
        public int TrajanjeMinut { get; set; }      // specifična lastnost 2

        public AvdioVizualnoGradivo(int id, string naslov, string avtor, int letoIzdaje,
                                      VrstaAVGradiva vrsta, int trajanjeMinut)
            : base(id, naslov, avtor, letoIzdaje)
        {
            Vrsta = vrsta;
            TrajanjeMinut = trajanjeMinut;
        }

        public override TipGradiva Tip => TipGradiva.AvdioVizualnoGradivo;

        public override string ToString()
            => base.ToString() + $" | Vrsta AV gradiva: {Vrsta}, Trajanje: {TrajanjeMinut} min";
    }

    // =====================================================================
    // ČLAN KNJIŽNICE
    // =====================================================================

    public class Clan
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Priimek { get; set; }

        public List<Gradivo> IzposojenoGradivo { get; set; }
        public List<Gradivo> VrnjenoGradivo { get; set; }

        public Clan(int id, string ime, string priimek)
        {
            Id = id;
            Ime = ime;
            Priimek = priimek;
            IzposojenoGradivo = new List<Gradivo>();
            VrnjenoGradivo = new List<Gradivo>();
        }

        public override string ToString() => $"{Ime} {Priimek} (ID {Id})";
    }

    // =====================================================================
    // KNJIŽNICA
    // =====================================================================

    public class Knjiznica
    {
        public string Ime { get; set; }
        public string Naslov { get; set; }
        public string KontaktniEmail { get; set; }

        // Izbrana podatkovna struktura: Dictionary<int, Gradivo>
        // -----------------------------------------------------------------
        // Po seznamu gradiv knjižnice se zelo veliko išče (rezervacija,
        // izposoja, vračanje, dodajanje) IZKLJUČNO po ID-ju gradiva, ki je
        // unikaten. Dictionary omogoča iskanje, dodajanje in brisanje v
        // povprečnem času O(1), medtem ko bi pri List<Gradivo> iskanje po
        // ID-ju zahtevalo linearen pregled O(n). Pri velikem številu
        // elementov (kar je v nalogi izrecno navedeno) je razlika v hitrosti
        // bistvena, zato je Dictionary<int, Gradivo> najprimernejša izbira.
        public Dictionary<int, Gradivo> Gradiva { get; set; }

        public List<Clan> Clani { get; set; }

        public Knjiznica(string ime, string naslov, string kontaktniEmail)
        {
            Ime = ime;
            Naslov = naslov;
            KontaktniEmail = kontaktniEmail;
            Gradiva = new Dictionary<int, Gradivo>();
            Clani = new List<Clan>();
        }

        // Izpiše vsa gradiva izbranega tipa (enumeracija TipGradiva), urejena
        // po letu izdaje, in vrne število izpisanih gradiv.
        public int IzpisiGradiva(TipGradiva tip)
        {
            var izbrana = Gradiva.Values
                .Where(g => g.Tip == tip)
                .OrderBy(g => g.LetoIzdaje)
                .ToList();

            Console.WriteLine($"--- Gradiva tipa {tip} v knjižnici '{Ime}' (urejena po letu izdaje) ---");
            foreach (var g in izbrana)
            {
                Console.WriteLine(g);
            }
            Console.WriteLine($"Skupno število gradiv tega tipa: {izbrana.Count}");
            Console.WriteLine();

            return izbrana.Count;
        }
    }

    // =====================================================================
    // FACTORY - oblikovalski vzorec za kreiranje novih gradiv
    // =====================================================================

    public static class GradivoFactory
    {
        // Glede na podan TipGradiva ustvari ustrezno konkretno instanco
        // gradiva. Parametra "specVrsta" in "specStevilo" predstavljata
        // specifični lastnosti posameznega tipa gradiva (npr. VrstaKnjige
        // in število strani, VrstaPublikacije in številko izdaje,
        // VrstaAVGradiva in trajanje v minutah).
        public static Gradivo UstvariGradivo(TipGradiva tip, int id, string naslov,
            string avtor, int letoIzdaje, object specVrsta, int specStevilo)
        {
            switch (tip)
            {
                case TipGradiva.Knjiga:
                    return new Knjiga(id, naslov, avtor, letoIzdaje,
                        (VrstaKnjige)specVrsta, specStevilo);

                case TipGradiva.PeriodicnaPublikacija:
                    return new PeriodicnaPublikacija(id, naslov, avtor, letoIzdaje,
                        (VrstaPublikacije)specVrsta, specStevilo);

                case TipGradiva.AvdioVizualnoGradivo:
                    return new AvdioVizualnoGradivo(id, naslov, avtor, letoIzdaje,
                        (VrstaAVGradiva)specVrsta, specStevilo);

                default:
                    throw new ArgumentException("Neznan tip gradiva.");
            }
        }
    }

    // =====================================================================
    // GLAVNI PROGRAM
    // =====================================================================

    public class Program
    {
        // Seznam vseh knjižnic, ki uporabljajo ta program
        public List<Knjiznica> Knjiznice { get; set; } = new List<Knjiznica>();

        // Pomožna metoda - poišče gradivo (po ID-ju) in člana (po ID-ju)
        // znotraj izbrane knjižnice
        private (Gradivo gradivo, Clan clan) PoisciGradivoInClana(
            Knjiznica knjiznica, int idGradiva, int idClana)
        {
            if (!knjiznica.Gradiva.TryGetValue(idGradiva, out var gradivo))
                throw new ArgumentException(
                    $"Gradivo z ID {idGradiva} ne obstaja v knjižnici '{knjiznica.Ime}'.");

            var clan = knjiznica.Clani.FirstOrDefault(c => c.Id == idClana);
            if (clan == null)
                throw new ArgumentException(
                    $"Član z ID {idClana} ne obstaja v knjižnici '{knjiznica.Ime}'.");

            return (gradivo, clan);
        }

        // Rezervacija gradiva za izbranega člana
        public void RezervirajGradivo(Knjiznica knjiznica, int idGradiva, int idClana)
        {
            var (gradivo, clan) = PoisciGradivoInClana(knjiznica, idGradiva, idClana);
            gradivo.Rezerviraj();
            Console.WriteLine($"{clan} je rezerviral/-a gradivo '{gradivo.Naslov}'.");
        }

        // Izposoja gradiva izbranemu članu
        public void IzposodiGradivo(Knjiznica knjiznica, int idGradiva, int idClana)
        {
            var (gradivo, clan) = PoisciGradivoInClana(knjiznica, idGradiva, idClana);
            gradivo.Izposodi();
            clan.IzposojenoGradivo.Add(gradivo);
            Console.WriteLine($"{clan} si je izposodil/-a gradivo '{gradivo.Naslov}'.");
        }

        // Vračanje gradiva, ki ga je izposodil izbrani član
        public void VrniGradivo(Knjiznica knjiznica, int idGradiva, int idClana)
        {
            var (gradivo, clan) = PoisciGradivoInClana(knjiznica, idGradiva, idClana);
            gradivo.Vrni();

            if (clan.IzposojenoGradivo.Contains(gradivo))
                clan.IzposojenoGradivo.Remove(gradivo);

            clan.VrnjenoGradivo.Add(gradivo);
            Console.WriteLine($"{clan} je vrnil/-a gradivo '{gradivo.Naslov}'.");
        }

        // Doda novo gradivo v izbrano knjižnico - kreiranje instance s
        // pomočjo oblikovalskega vzorca Factory
        public void DodajGradivo(Knjiznica knjiznica, TipGradiva tip, int id,
            string naslov, string avtor, int letoIzdaje, object specVrsta, int specStevilo)
        {
            var novoGradivo = GradivoFactory.UstvariGradivo(
                tip, id, naslov, avtor, letoIzdaje, specVrsta, specStevilo);

            knjiznica.Gradiva.Add(id, novoGradivo);
            Console.WriteLine($"Gradivo '{naslov}' je bilo dodano v knjižnico '{knjiznica.Ime}'.");
        }

        // Statična metoda: za dano knjižnico vrne skupno število trenutno
        // izposojenega gradiva vseh članov - izključno z uporabo LINQ
        public static int SteviloIzposojenihGradiv(Knjiznica knjiznica)
        {
            return knjiznica.Clani.Sum(c => c.IzposojenoGradivo.Count);
        }

        public static void Main(string[] args)
        {
            var program = new Program();

            // (5) Ustvarimo eno knjižnico
            var osKnjiznica = new Knjiznica(
                "Knjižnica OŠ Bratstvo", "Šolska ulica 1, 2250 Ptuj", "knjiznica@os-bratstvo.si");
            program.Knjiznice.Add(osKnjiznica);

            // Dodamo gradiva - vsaj pet gradiv, vsaj treh tipov (Factory)
            program.DodajGradivo(osKnjiznica, TipGradiva.Knjiga, 1,
                "Mali princ", "Antoine de Saint-Exupéry", 1943, VrstaKnjige.Roman, 96);

            program.DodajGradivo(osKnjiznica, TipGradiva.Knjiga, 2,
                "Veliki splošni leksikon", "DZS", 1997, VrstaKnjige.Leksikon, 1500);

            program.DodajGradivo(osKnjiznica, TipGradiva.Knjiga, 3,
                "Mavrica", "Ela Peroci", 1966, VrstaKnjige.Slikanica, 24);

            program.DodajGradivo(osKnjiznica, TipGradiva.PeriodicnaPublikacija, 4,
                "PIL", "Uredništvo PIL", 2024, VrstaPublikacije.Revija, 245);

            program.DodajGradivo(osKnjiznica, TipGradiva.AvdioVizualnoGradivo, 5,
                "Pomladna pravljica", "RTV Slovenija", 2010, VrstaAVGradiva.DVD, 90);

            // Dodamo dva člana
            var clan1 = new Clan(1, "Ana", "Kovač");
            var clan2 = new Clan(2, "Jan", "Novak");
            osKnjiznica.Clani.Add(clan1);
            osKnjiznica.Clani.Add(clan2);

            Console.WriteLine();

            // Rezervacija in izposoja
            program.RezervirajGradivo(osKnjiznica, 2, clan1.Id); // Ana rezervira leksikon
            program.IzposodiGradivo(osKnjiznica, 1, clan2.Id);   // Jan si izposodi Malega princa

            Console.WriteLine();

            // Izpis gradiv po posameznih tipih (enumeracija TipGradiva)
            osKnjiznica.IzpisiGradiva(TipGradiva.Knjiga);
            osKnjiznica.IzpisiGradiva(TipGradiva.PeriodicnaPublikacija);
            osKnjiznica.IzpisiGradiva(TipGradiva.AvdioVizualnoGradivo);

            // (6) Statična LINQ metoda - skupno število izposojenega gradiva
            int steviloIzposojenih = SteviloIzposojenihGradiv(osKnjiznica);
            Console.WriteLine($"Skupno število trenutno izposojenega gradiva v knjižnici: {steviloIzposojenih}");
        }
    }
}
