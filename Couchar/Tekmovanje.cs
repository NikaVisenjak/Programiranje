using System.Collections.Generic;
using System.Linq;

namespace Couchar
{
    public class Tekmovanje
    {
        // Readonly - ime tekmovanja se po kreiranju ne spremeni.
        public string Ime { get; }

        public TipTekmovanja Tip { get; set; }
        public SistemTekmovanja Sistem { get; set; }

        // Readonly - sport tekmovanja je dolocen ob kreiranju in se ne menja
        // (tekme in statistike so odvisne od njega).
        public Sport Sport { get; }

        private readonly List<Ekipa> ekipe = new List<Ekipa>();
        public IReadOnlyList<Ekipa> Ekipe => ekipe;

        private readonly List<Tekma> tekme = new List<Tekma>();
        public IReadOnlyList<Tekma> Tekme => tekme;

        public Tekmovanje(string ime, TipTekmovanja tip, SistemTekmovanja sistem, Sport sport)
        {
            Ime = ime;
            Tip = tip;
            Sistem = sistem;
            Sport = sport;
        }

        public void DodajEkipo(Ekipa ekipa) => ekipe.Add(ekipa);

        public void DodajTekmo(Tekma tekma) => tekme.Add(tekma);

        /// <summary>
        /// (4) Izracuna trenutni vrstni red ekip na tekmovanju.
        /// Funkcija je neodvisna od konkretnega sporta, ker se za
        /// pretvorbo statistike v tocke uporabi Sport.IzracunajTocke
        /// (vmesnik ITockovanje) - vsak sport sam definira svoja pravila.
        /// </summary>
        public List<Ekipa> IzracunajVrstniRed()
        {
            var tockeEkip = ekipe.ToDictionary(e => e, e => 0);

            foreach (var tekma in tekme)
            {
                int tocke1 = Sport.IzracunajTocke(tekma.Statistika1, tekma.Statistika2);
                int tocke2 = Sport.IzracunajTocke(tekma.Statistika2, tekma.Statistika1);

                if (tockeEkip.ContainsKey(tekma.Ekipa1))
                    tockeEkip[tekma.Ekipa1] += tocke1;

                if (tockeEkip.ContainsKey(tekma.Ekipa2))
                    tockeEkip[tekma.Ekipa2] += tocke2;
            }

            return tockeEkip
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => kv.Key.Ime)
                .Select(kv => kv.Key)
                .ToList();
        }

        /// <summary>
        /// (7) Vrne seznam vseh tekem, v katerih je nastopila dana ekipa.
        /// Uporablja LINQ (Where) in lambda izraz.
        /// </summary>
        public List<Tekma> TekmeEkipe(Ekipa ekipa)
        {
            return tekme
                .Where(t => t.Ekipa1 == ekipa || t.Ekipa2 == ekipa)
                .ToList();
        }

        public override string ToString() => $"{Ime} ({Tip}, {Sport})";
    }
}
