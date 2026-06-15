using System;
using CinemaSystem.Models;
using CinemaSystem.Extensions;

namespace CinemaSystem
{
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
