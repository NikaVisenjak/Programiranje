using System.Linq;
using BolnisnicaApp.Models;

namespace BolnisnicaApp.Extensions
{
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
}
