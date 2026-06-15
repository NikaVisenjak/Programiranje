using System;
using System.Linq;
using KnjiznicaApp.Enumeracije;
using KnjiznicaApp.Modeli;
using KnjiznicaApp.Tovarna;
using System.Collections.Generic;

namespace KnjiznicaApp
{
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
