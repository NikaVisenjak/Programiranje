using System;

namespace Couchar
{
    /// <summary>
    /// (3) Razred uporabniskega vmesnika, ki s pomocjo Factory vzorca
    /// uporabniku omogoca vnos novega tekmovanja in ekip, ki v njem nastopajo.
    /// Factory vzorec se uporabi za kreiranje pravega podrazreda Sport
    /// (Nogomet/Kosarka) glede na izbiro uporabnika, brez da bi UI
    /// poznal konkretne razrede sportov.
    /// </summary>
    public class UporabniskiVmesnik
    {
        public Tekmovanje VnesiTekmovanje()
        {
            Console.WriteLine("=== Vnos novega tekmovanja ===");

            Console.Write("Ime tekmovanja: ");
            string ime = Console.ReadLine();

            Console.Write("Vrsta sporta (0 - Nogomet, 1 - Kosarka): ");
            VrstaSporta vrstaSporta = (VrstaSporta)int.Parse(Console.ReadLine());

            Console.Write("Tip tekmovanja (0-Evropsko, 1-Svetovno, 2-Drzavno, 3-Regionalno, 4-Lokalno): ");
            TipTekmovanja tip = (TipTekmovanja)int.Parse(Console.ReadLine());

            Console.Write("Sistem tekmovanja (0-Ligaski, 1-Pokalni, 2-Skupinski): ");
            SistemTekmovanja sistem = (SistemTekmovanja)int.Parse(Console.ReadLine());

            // --- uporaba Factory vzorca ---
            SportFactory factory = SportFactoryPonudnik.PridobiFactory(vrstaSporta);
            Sport sport = factory.UstvariSport();
            // --------------------------------

            var tekmovanje = new Tekmovanje(ime, tip, sistem, sport);

            Console.Write("Stevilo ekip, ki jih zelite vnesti: ");
            int steviloEkip = int.Parse(Console.ReadLine());

            for (int i = 0; i < steviloEkip; i++)
            {
                Console.WriteLine($"--- Ekipa {i + 1} ---");

                Console.Write("Ime ekipe: ");
                string imeEkipe = Console.ReadLine();

                Console.Write("Drzava: ");
                string drzava = Console.ReadLine();

                Console.Write("Tip ekipe (0-Klub, 1-DrzavnaReprezentanca): ");
                TipEkipe tipEkipe = (TipEkipe)int.Parse(Console.ReadLine());

                Ekipa ekipa;
                if (tipEkipe == TipEkipe.Klub)
                {
                    Console.Write("Stadion: ");
                    string stadion = Console.ReadLine();

                    Console.Write("Sponzor: ");
                    string sponzor = Console.ReadLine();

                    ekipa = new Klub(imeEkipe, drzava, stadion, sponzor);
                }
                else
                {
                    Console.Write("Selektor: ");
                    string selektor = Console.ReadLine();

                    ekipa = new DrzavnaReprezentanca(imeEkipe, drzava, selektor);
                }

                tekmovanje.DodajEkipo(ekipa);
            }

            return tekmovanje;
        }
    }
}
