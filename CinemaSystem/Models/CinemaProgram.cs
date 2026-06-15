using System;
using System.Collections.Generic;
using System.Linq;
using CinemaSystem.Interfaces;

namespace CinemaSystem.Models
{
    // =====================================================================
    // Razred CinemaProgram (program/spored posamezne dvorane)
    // Opomba: poimenovan "CinemaProgram" namesto zgolj "Program", da se izognemo
    // kolizijam imen z razredom Program, ki v projektu vsebuje metodo Main.
    //
    // - Entries -> ne-avtomatsko implementirana lastnost (zasebno polje + dostopnik)
    // - Hall    -> avtomatsko implementirana lastnost (povezava nazaj na dvorano)
    // =====================================================================
    public class CinemaProgram : IProgram
    {
        // Ne-avtomatsko implementirana lastnost
        private List<ProgramEntry> _entries = new List<ProgramEntry>();
        public List<ProgramEntry> Entries
        {
            get => _entries;
        }

        // Avtomatsko implementirana lastnost - povezava na dvorano, ki ji program pripada
        public Hall Hall { get; set; }

        public void AddEntry(DateTime date, TimeSpan time, Movie movie)
        {
            _entries.Add(new ProgramEntry { Date = date.Date, Time = time, Movie = movie });
        }

        // (3) Implementacija funkcije iz vmesnika IProgram
        public bool IsEmpty(DateTime date, TimeSpan time, TimeSpan interval)
        {
            DateTime intervalStart = date.Date + time;
            DateTime intervalEnd = intervalStart + interval;

            // Preverimo, ali se kateri od obstoječih terminov prekriva s podanim intervalom
            bool zaseden = _entries.Any(e =>
                e.Date.Date == date.Date &&
                e.StartDateTime < intervalEnd &&
                e.EndDateTime > intervalStart);

            return !zaseden;
        }
    }
}
