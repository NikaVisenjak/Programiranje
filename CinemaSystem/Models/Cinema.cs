using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem.Models
{
    // =====================================================================
    // Razred Cinema (kinodvorana / stavba)
    // - Name, City -> avtomatsko implementirani lastnosti
    // - Halls      -> ne-avtomatsko implementirana lastnost (zasebno polje + dostopnik)
    // - Movies     -> avtomatsko implementirana lastnost
    // =====================================================================
    public class Cinema
    {
        public string Name { get; set; }
        public string City { get; set; }

        // Ne-avtomatsko implementirana lastnost
        private List<Hall> _halls = new List<Hall>();
        public List<Hall> Halls => _halls;

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public Cinema(string name, string city)
        {
            Name = name;
            City = city;
        }

        public void AddHall(Hall hall) => _halls.Add(hall);
        public void AddMovie(Movie movie) => Movies.Add(movie);

        // (5) Izpis vseh prostih dvoran v kinu za dani datum, uro in interval (z uporabo LINQ)
        public void PrintFreeHalls(DateTime date, TimeSpan time, TimeSpan interval)
        {
            var freeHalls = _halls
                .Where(h => h.Program.IsEmpty(date, time, interval))
                .ToList();

            Console.WriteLine($"Proste dvorane v kinu '{Name}' dne {date:dd.MM.yyyy} ob {time} za {interval.TotalMinutes} min:");

            if (!freeHalls.Any())
                Console.WriteLine("  (ni prostih dvoran)");
            else
                freeHalls.ForEach(h => Console.WriteLine($"  - {h.Name}"));
        }

        // (8) Za vse dvorane kina zažene PlayMovie glede na vnos v programu
        // za dani datum in uro (brez preverjanja, ali je film res na sporedu)
        public void StartAllHalls(DateTime date, TimeSpan time)
        {
            Console.WriteLine($"Zagon predvajanj v kinu '{Name}' dne {date:dd.MM.yyyy} ob {time}:");

            foreach (var hall in _halls)
            {
                var entry = hall.Program.Entries
                    .FirstOrDefault(e => e.Date.Date == date.Date && e.Time == time);

                if (entry != null)
                    hall.PlayMovie(entry.Movie);
                else
                    Console.WriteLine($"  -> Dvorana '{hall.Name}': ni vnosa v programu za ta termin.");
            }
        }
    }
}
