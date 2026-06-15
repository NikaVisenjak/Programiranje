namespace Couchar
{
    /// <summary>
    /// Abstraktni razred, ki predstavlja sport. Vsak konkretni sport
    /// (Nogomet, Kosarka, ...) ve, kaksen tip statistike uporablja in
    /// kako iz statistike izracunati tocke za vrstni red (ITockovanje).
    /// </summary>
    public abstract class Sport : ITockovanje
    {
        // Readonly lastnosti - dolocene ob kreiranju konkretnega sporta.
        public string Ime { get; }
        public VrstaSporta Vrsta { get; }

        protected Sport(string ime, VrstaSporta vrsta)
        {
            Ime = ime;
            Vrsta = vrsta;
        }

        /// <summary>Tovarniska metoda - vsak sport ustvari "svojo" statistiko.</summary>
        public abstract Statistika UstvariStatistiko(Tekma tekma);

        /// <summary>Pretvorba statistike v tocke za vrstni red - odvisno od sporta.</summary>
        public abstract int IzracunajTocke(Statistika lastnaStatistika, Statistika statistikaNasprotnika);

        public override string ToString() => Ime;
    }
}
