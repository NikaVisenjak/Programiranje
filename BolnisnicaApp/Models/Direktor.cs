using System;

namespace BolnisnicaApp.Models
{
    /// <summary>
    /// Razred predstavlja direktorja bolnišnice. V sistemu lahko obstaja
    /// samo ena instanca tega razreda, zato je uporabljen oblikovalski
    /// vzorec Singleton (Naloga 3).
    /// </summary>
    public class Direktor : Zaposleni
    {
        // Edina (statična) instanca razreda Direktor.
        private static Direktor instance;

        // Generator naključnih ocen pri razgovorih.
        private static readonly Random random = new Random();

        // Privaten konstruktor preprečuje ustvarjanje instanc od zunaj,
        // s čimer je zagotovljeno, da je v sistemu lahko ustvarjena
        // samo ena instanca razreda Direktor.
        private Direktor(string ime, string priimek, DateTime datumZaposlitve)
            : base(ime, priimek, datumZaposlitve)
        {
        }

        /// <summary>
        /// Vrne edino instanco razreda Direktor. Ob prvem klicu jo ustvari
        /// s podanimi podatki, vsi nadaljnji klici pa vrnejo isto instanco
        /// (parametri se pri naslednjih klicih ignorirajo).
        /// </summary>
        public static Direktor Instance(string ime = "Igor", string priimek = "Sinkovec", DateTime? datumZaposlitve = null)
        {
            if (instance == null)
            {
                instance = new Direktor(ime, priimek, datumZaposlitve ?? DateTime.Now);
            }

            return instance;
        }

        /// <summary>
        /// Opravi razgovor z izbranim zaposlenim in vrne oceno zaposlenega
        /// po razgovoru - celo število med 1 in 5.
        /// </summary>
        public int Razgovor(Zaposleni zaposleni)
        {
            int ocena = random.Next(1, 6); // Next(1, 6) vrne vrednosti 1-5
            Console.WriteLine($"Direktor {Ime} {Priimek} je opravil razgovor z {zaposleni} - ocena: {ocena}/5.");
            return ocena;
        }

        public override string OpisDelovnihNalog()
        {
            return $"Direktor {Ime} {Priimek} vodi bolnišnico in vsako leto opravi razgovore z vsemi zaposlenimi.";
        }
    }
}
