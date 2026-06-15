using System;
using System.Collections.Generic;
using System.Linq;
using KnjiznicaApp.Enumeracije;

namespace KnjiznicaApp.Modeli
{
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
}
