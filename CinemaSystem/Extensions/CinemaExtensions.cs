using System;
using System.Linq;
using CinemaSystem.Models;

namespace CinemaSystem.Extensions
{
    // =====================================================================
    // (7) Razširitvena metoda za razred Cinema
    // (uporabljena, ker razreda Cinema ne moremo več spreminjati)
    // =====================================================================
    public static class CinemaExtensions
    {
        // Vrne število vseh filmov, ki se v izbranem kinu predvajajo
        // na dani datum in uro (preko vseh dvoran)
        public static int CountMoviesAt(this Cinema cinema, DateTime date, TimeSpan time)
        {
            return cinema.Halls
                .SelectMany(h => h.Program.Entries)
                .Count(e => e.Date.Date == date.Date && e.Time == time);
        }
    }
}
