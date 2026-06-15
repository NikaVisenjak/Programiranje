using System;

namespace Couchar
{
    public class Program
    {
        public static void Main()
        {
            // --- Kreiranje sporta in tekmovanja ---
            Sport nogomet = new Nogomet();
            var em = new Tekmovanje(
                "Evropsko prvenstvo 2024",
                TipTekmovanja.Evropsko,
                SistemTekmovanja.Skupinski,
                nogomet);

            // --- Kreiranje ekip ---
            var slovenija = new DrzavnaReprezentanca("Slovenija", "Slovenija", "Matjaz Kek");
            var anglija = new DrzavnaReprezentanca("Anglija", "Anglija", "Gareth Southgate");
            var danska = new DrzavnaReprezentanca("Danska", "Danska", "Kasper Hjulmand");

            em.DodajEkipo(slovenija);
            em.DodajEkipo(anglija);
            em.DodajEkipo(danska);

            // --- Rocno kreiranje nekaj tekem in vnos statistike ---
            var tekma1 = new Tekma(em, slovenija, danska, new DateTime(2024, 6, 16));
            ((NogometnaStatistika)tekma1.Statistika1).Goli = 1;
            ((NogometnaStatistika)tekma1.Statistika2).Goli = 1;
            em.DodajTekmo(tekma1);

            var tekma2 = new Tekma(em, anglija, slovenija, new DateTime(2024, 6, 25));
            ((NogometnaStatistika)tekma2.Statistika1).Goli = 0;
            ((NogometnaStatistika)tekma2.Statistika2).Goli = 0;
            em.DodajTekmo(tekma2);

            var tekma3 = new Tekma(em, danska, anglija, new DateTime(2024, 6, 20));
            ((NogometnaStatistika)tekma3.Statistika1).Goli = 1;
            ((NogometnaStatistika)tekma3.Statistika2).Goli = 1;
            em.DodajTekmo(tekma3);

            // --- (5) Izpis trenutnega vrstnega reda ---
            Console.WriteLine("Trenutni vrstni red:");
            var vrstniRed = em.IzracunajVrstniRed();
            for (int i = 0; i < vrstniRed.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {vrstniRed[i]}");
            }

            // --- (6) Primer uporabe razsiritvene metode IzpisiStatistiko ---
            Console.WriteLine();
            Console.WriteLine(slovenija.IzpisiStatistiko(tekma1.Statistika1));

            // --- (7) Primer uporabe TekmeEkipe ---
            Console.WriteLine();
            Console.WriteLine("Tekme Slovenije:");
            foreach (var t in em.TekmeEkipe(slovenija))
            {
                Console.WriteLine(t);
            }
        }
    }
}
