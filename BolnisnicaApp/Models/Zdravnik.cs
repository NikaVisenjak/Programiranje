using System;
using System.Collections.Generic;

namespace BolnisnicaApp.Models
{
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
}
