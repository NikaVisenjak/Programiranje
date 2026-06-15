using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem
{
    // =====================================================================
    // (2) Enumeracija žanrov
    // =====================================================================
    public enum Genre
    {
        Komedija,
        Triler,
        Grozljivka,
        Drama,
        Akcija,
        Animirani,
        ZnanstvenaFantastika
    }

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

    // =====================================================================
    // Pomožni razred - posamezen vnos v programu dvorane
    // (datum, ura, film, ki se takrat predvaja)
    // =====================================================================
    public class ProgramEntry
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Movie Movie { get; set; }

        // Pomožna izračunana lastnost - čas pričetka in konca predvajanja
        public DateTime StartDateTime => Date.Date + Time;
        public DateTime EndDateTime => StartDateTime + Movie.Duration;
    }

    // =====================================================================
    // (3) Vmesnik IProgram
    // =====================================================================
    public interface IProgram
    {
        // Vrne true, če je dvorana v podanem datumu/času/intervalu prosta
        bool IsEmpty(DateTime date, TimeSpan time, TimeSpan interval);
    }

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

    // =====================================================================
    // (6) Razred Member (registriran obiskovalec)
    // =====================================================================
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        // Dodatna (4.) lastnost - npr. točke zvestobe
        public int LoyaltyPoints { get; set; }
    }

    // =====================================================================
    // Razred CineCompany (podjetje)
    // - Name    -> avtomatsko implementirana lastnost
    // - Cinemas -> ne-avtomatsko implementirana lastnost (zasebno polje + dostopnik)
    // - Members -> avtomatsko implementirana lastnost (Dictionary za hitro iskanje)
    // =====================================================================
    public class CineCompany
    {
        public string Name { get; set; }

        // Ne-avtomatsko implementirana lastnost
        private List<Cinema> _cinemas = new List<Cinema>();
        public List<Cinema> Cinemas => _cinemas;

        // (6) Registrirani obiskovalci shranjeni v slovarju (Dictionary<int, Member>),
        // kjer je ključ ID člana.
        // Izbrana podatkovna struktura: Dictionary
        // Razlog: iskanje, dodajanje in posodabljanje člana po ID-ju je v povprečju O(1),
        // medtem ko bi iskanje v navadnem List<Member> zahtevalo O(n) - linearni pregled.
        // Ker pri velikem številu članov pričakujemo pogosto iskanje po ID-ju
        // (npr. ob nakupu vstopnice, prijavi v sistem...), je Dictionary
        // bistveno učinkovitejša izbira.
        public Dictionary<int, Member> Members { get; set; } = new Dictionary<int, Member>();

        public CineCompany(string name)
        {
            Name = name;
        }

        public void AddCinema(Cinema cinema) => _cinemas.Add(cinema);

        public void AddMember(Member member) => Members[member.Id] = member;
    }

    // =====================================================================
    // (7) Razširitvena metoda za razred Cinema
    // (uporabljena, ker razreda Cinema ne moremo več spreminjati)
    // =====================================================================
    public static class CinemaExtensions
    {
        // Izpiše/vrne število vseh filmov, ki se v izbranem kinu predvajajo
        // na dani datum in uro (preko vseh dvoran)
        public static int CountMoviesAt(this Cinema cinema, DateTime date, TimeSpan time)
        {
            return cinema.Halls
                .SelectMany(h => h.Program.Entries)
                .Count(e => e.Date.Date == date.Date && e.Time == time);
        }
    }

    // =====================================================================
    // (4) Glavni razred programa
    // =====================================================================
    class Program
    {
        static void Main(string[] args)
        {
            // --- Podjetje ---
            var company = new CineCompany("KinoSI");

            // --- Filmi ---
            var movie1 = new Movie("Vesoljska odiseja", Genre.ZnanstvenaFantastika, TimeSpan.FromMinutes(120));
            var movie2 = new Movie("Smeh do solz", Genre.Komedija, TimeSpan.FromMinutes(95));
            var movie3 = new Movie("Nočna grozljivka", Genre.Grozljivka, TimeSpan.FromMinutes(105));
            var movie4 = new Movie("Hitra zasleda", Genre.Akcija, TimeSpan.FromMinutes(110));
            var movie5 = new Movie("Skrivnost preteklosti", Genre.Triler, TimeSpan.FromMinutes(100));
            var movie6 = new Movie("Risani prijatelji", Genre.Animirani, TimeSpan.FromMinutes(90));

            var datum = new DateTime(2026, 6, 15);

            // --- Kino 1: Ljubljana (2 dvorani, 3 filmi) ---
            var cinemaLJ = new Cinema("Kino Ljubljana", "Ljubljana");
            var hallLJ1 = new Hall("Dvorana 1", 150);
            var hallLJ2 = new Hall("Dvorana 2", 100);

            cinemaLJ.AddHall(hallLJ1);
            cinemaLJ.AddHall(hallLJ2);

            cinemaLJ.AddMovie(movie1);
            cinemaLJ.AddMovie(movie2);
            cinemaLJ.AddMovie(movie3);

            hallLJ1.Program.AddEntry(datum, new TimeSpan(18, 0, 0), movie1);
            hallLJ1.Program.AddEntry(datum, new TimeSpan(21, 0, 0), movie2);
            hallLJ2.Program.AddEntry(datum, new TimeSpan(20, 0, 0), movie3);

            // --- Kino 2: Maribor (2 dvorani, 3 filmi) ---
            var cinemaMB = new Cinema("Kino Maribor", "Maribor");
            var hallMB1 = new Hall("Dvorana A", 200);
            var hallMB2 = new Hall("Dvorana B", 80);

            cinemaMB.AddHall(hallMB1);
            cinemaMB.AddHall(hallMB2);

            cinemaMB.AddMovie(movie4);
            cinemaMB.AddMovie(movie5);
            cinemaMB.AddMovie(movie6);

            hallMB1.Program.AddEntry(datum, new TimeSpan(19, 0, 0), movie4);
            hallMB2.Program.AddEntry(datum, new TimeSpan(17, 30, 0), movie5);
            hallMB2.Program.AddEntry(datum, new TimeSpan(20, 30, 0), movie6);

            // --- Dodajanje kinov v podjetje ---
            company.AddCinema(cinemaLJ);
            company.AddCinema(cinemaMB);

            // --- (6) Registracija članov ---
            company.AddMember(new Member { Id = 1, Name = "Ana Novak", Email = "ana.novak@email.com", LoyaltyPoints = 120 });
            company.AddMember(new Member { Id = 2, Name = "Jan Kovač", Email = "jan.kovac@email.com", LoyaltyPoints = 45 });

            // Primer hitrega iskanja člana po ID-ju
            if (company.Members.TryGetValue(1, out var member))
                Console.WriteLine($"Najden član: {member.Name} ({member.Email})\n");

            // --- (5) Izpis prostih dvoran ---
            cinemaLJ.PrintFreeHalls(datum, new TimeSpan(18, 30, 0), TimeSpan.FromMinutes(60));
            Console.WriteLine();

            // --- (7) Klic razširitvene metode ---
            int steviloFilmov = cinemaLJ.CountMoviesAt(datum, new TimeSpan(18, 0, 0));
            Console.WriteLine($"Število filmov, ki se predvajajo {datum:dd.MM.yyyy} ob 18:00 v kinu '{cinemaLJ.Name}': {steviloFilmov}\n");

            // --- (8) Zagon predvajanja v vseh dvoranah izbranega kina ---
            cinemaLJ.StartAllHalls(datum, new TimeSpan(18, 0, 0));
        }
    }
}
