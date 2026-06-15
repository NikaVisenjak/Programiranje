using System;

namespace CinemaSystem.Models
{
    // =====================================================================
    // Razred Hall (dvorana)
    // - Name     -> avtomatsko implementirana lastnost
    // - Capacity -> ne-avtomatsko implementirana lastnost (z validacijo)
    // - Program  -> avtomatsko implementirana lastnost (povezava na CinemaProgram)
    // =====================================================================
    public class Hall
    {
        public string Name { get; set; }

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Kapaciteta dvorane mora biti pozitivna.");
                _capacity = value;
            }
        }

        public CinemaProgram Program { get; set; }

        public Hall(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            Program = new CinemaProgram { Hall = this };
        }

        // (8) Funkcija PlayMovie - predstavlja pričetek predvajanja filma
        public void PlayMovie(Movie movie)
        {
            Console.WriteLine($"  -> Dvorana '{Name}': pričenjam predvajanje filma '{movie.Title}'.");
        }
    }
}
