using System;
using System.Collections.Generic;
using System.Linq;
using BolnisnicaApp.Interfaces;

namespace BolnisnicaApp.Models
{
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
}
