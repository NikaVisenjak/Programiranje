using System;

namespace CinemaSystem.Models
{
    // =====================================================================
    // Razred Movie (film)
    // - Title in Genre  -> avtomatsko implementirani lastnosti
    // - Duration        -> ne-avtomatsko implementirana lastnost (z validacijo)
    // =====================================================================
    public class Movie
    {
        // Avtomatsko implementirana lastnost
        public string Title { get; set; }

        // (2) Žanr filma, vrednost iz enumeracije Genre - avtomatsko implementirana lastnost
        public Genre Genre { get; set; }

        // Ne-avtomatsko implementirana lastnost (ima zasebno polje in validacijo v setterju)
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentException("Trajanje filma mora biti pozitivno.");
                _duration = value;
            }
        }

        public Movie(string title, Genre genre, TimeSpan duration)
        {
            Title = title;
            Genre = genre;
            Duration = duration;
        }

        public override string ToString() => $"{Title} ({Genre}, {Duration.TotalMinutes} min)";
    }
}
