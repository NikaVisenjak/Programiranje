using System;
using System.Linq;
using BolnisnicaApp.Models;
using BolnisnicaApp.Interfaces;
using BolnisnicaApp.Extensions;

namespace BolnisnicaApp
{
    /// <summary>
    /// Glavni razred sistema - vstopna točka aplikacije (Naloga 4).
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // ----------------------------------------------------------
            // NALOGA 4: Ustvarimo bolnišnico, vsaj 2 zdravnika, vsaj 2
            // medicinski sestri in vsaj 3 paciente, ter napolnimo
            // njihove lastnosti.
            // ----------------------------------------------------------
            Bolnisnica bolnisnica = new Bolnisnica("Splošna bolnišnica Novo mesto");

            Zdravnik zdravnik1 = new Zdravnik("Ana", "Kovač", new DateTime(2015, 3, 1), "Kardiologija");
            Zdravnik zdravnik2 = new Zdravnik("Marko", "Zupan", new DateTime(2018, 9, 15), "Kirurgija");

            MedicinskaSestra sestra1 = new MedicinskaSestra("Nina", "Horvat", new DateTime(2019, 5, 10), "Kardiologija");
            MedicinskaSestra sestra2 = new MedicinskaSestra("Tjaša", "Krajnc", new DateTime(2020, 1, 20), "Kirurgija");

            Pacient pacient1 = new Pacient("Janez", "Novak", new DateTime(1980, 4, 12));
            Pacient pacient2 = new Pacient("Maja", "Potočnik", new DateTime(1992, 7, 23));
            Pacient pacient3 = new Pacient("Peter", "Kralj", new DateTime(1975, 11, 5));

            // Zdravniki dobijo paciente, ki jih zdravijo.
            zdravnik1.Pacienti.Add(pacient1);
            zdravnik1.Pacienti.Add(pacient2);
            zdravnik2.Pacienti.Add(pacient3);
            zdravnik2.Pacienti.Add(pacient1);

            // Medicinske sestre dobijo dodeljene paciente, ki jih negujejo.
            sestra1.DodeljeniPacienti.Add(pacient1);
            sestra1.DodeljeniPacienti.Add(pacient2);
            sestra2.DodeljeniPacienti.Add(pacient3);

            // Dodamo nekaj zapisov v kartoteko pregledov pacientov.
            pacient1.DodajPregled("Pregled srca - vse v redu.");
            pacient2.DodajPregled("Splošni pregled.");
            pacient3.DodajPregled("Pooperativni pregled.");

            // ----------------------------------------------------------
            // NALOGA 2: Naročimo paciente na termine za določene datume.
            // ----------------------------------------------------------
            DateTime danes = DateTime.Today;
            DateTime jutri = danes.AddDays(1);

            zdravnik1.NarociPacienta(danes, pacient1);
            zdravnik1.NarociPacienta(danes, pacient2);
            zdravnik2.NarociPacienta(danes, pacient3);
            zdravnik2.NarociPacienta(jutri, pacient1);

            // Bolnišnica dobi seznam zaposlenih in seznam pacientov.
            bolnisnica.DodajZaposlenega(zdravnik1);
            bolnisnica.DodajZaposlenega(zdravnik2);
            bolnisnica.DodajZaposlenega(sestra1);
            bolnisnica.DodajZaposlenega(sestra2);

            bolnisnica.Pacienti.Add(pacient1);
            bolnisnica.Pacienti.Add(pacient2);
            bolnisnica.Pacienti.Add(pacient3);

            Console.WriteLine("=== Opisi delovnih nalog zaposlenih ===");
            foreach (Zaposleni z in bolnisnica.SeznamZaposlenih)
            {
                Console.WriteLine(z.OpisDelovnihNalog());
            }

            // ----------------------------------------------------------
            // NALOGA 3: Direktor je Singleton - obstaja samo ena instanca.
            // ----------------------------------------------------------
            Console.WriteLine("\n=== Razgovori z direktorjem ===");
            Direktor direktor = Direktor.Instance("Igor", "Sinkovec", new DateTime(2010, 1, 1));
            direktor.Razgovor(zdravnik1);
            direktor.Razgovor(sestra2);

            // Preverimo, da gre vedno za isto instanco.
            Direktor direktor2 = Direktor.Instance();
            Console.WriteLine($"Gre za isto instanco direktorja: {ReferenceEquals(direktor, direktor2)}");

            // ----------------------------------------------------------
            // NALOGA 5: Izpis vseh pacientov, ki imajo na dani datum
            // pregled pri katerem koli zdravniku (z uporabo LINQ).
            // ----------------------------------------------------------
            Console.WriteLine();
            IzpisiPacienteNaDan(bolnisnica, danes);
            IzpisiPacienteNaDan(bolnisnica, jutri);

            // ----------------------------------------------------------
            // NALOGA 6: Uporaba vmesnika IZadolzen.
            // ----------------------------------------------------------
            Console.WriteLine("\n=== Delovne naloge medicinske sestre ===");

            // PrevzemiNalogo je eksplicitna implementacija, zato jo kličemo
            // preko spremenljivke/referenece tipa IZadolzen.
            IZadolzen zadolzenaSestra = sestra1;
            zadolzenaSestra.PrevzemiNalogo("Priprava sobe za novega pacienta");
            zadolzenaSestra.PrevzemiNalogo("Razdelitev jutranjih zdravil");

            sestra1.ZakljuciNalogo(1);
            sestra1.PrenesiNalogo(2, sestra2);

            // ----------------------------------------------------------
            // NALOGA 7: Razširitvena funkcija - povprečno število
            // pacientov na zdravnika v bolnišnici.
            // ----------------------------------------------------------
            Console.WriteLine("\n=== Statistika ===");
            double povprecje = bolnisnica.PovprecnoPacientovNaZdravnika();
            Console.WriteLine($"Povprečno število pacientov na zdravnika v bolnišnici '{bolnisnica.Naziv}': {povprecje:F2}");
        }

        /// <summary>
        /// NALOGA 5: Statična funkcija, ki za dani datum in bolnišnico
        /// z uporabo LINQ izpiše seznam vseh pacientov, ki imajo na ta
        /// dan pregled pri katerem koli zdravniku.
        /// </summary>
        static void IzpisiPacienteNaDan(Bolnisnica bolnisnica, DateTime datum)
        {
            var pacientiNaDan = bolnisnica.SeznamZaposlenih
                .OfType<Zdravnik>()
                .Where(z => z.TerminiPoDatumih.ContainsKey(datum.Date))
                .SelectMany(z => z.TerminiPoDatumih[datum.Date])
                .Distinct()
                .ToList();

            Console.WriteLine($"Pacienti z pregledom na dan {datum:dd.MM.yyyy}:");

            if (pacientiNaDan.Count == 0)
            {
                Console.WriteLine("  (ni naročenih pacientov)");
            }
            else
            {
                foreach (Pacient p in pacientiNaDan)
                {
                    Console.WriteLine($"  - {p}");
                }
            }
        }
    }
}
