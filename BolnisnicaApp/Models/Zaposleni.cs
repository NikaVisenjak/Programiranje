using System;

namespace BolnisnicaApp.Models
{
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
}
