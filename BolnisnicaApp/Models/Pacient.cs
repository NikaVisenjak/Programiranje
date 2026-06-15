using System;
using System.Collections.Generic;

namespace BolnisnicaApp.Models
{
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
}
