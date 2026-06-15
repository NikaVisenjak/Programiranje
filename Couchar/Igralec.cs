namespace Couchar
{
    /// <summary>Posamezen igralec, ki nastopa za doloceno ekipo.</summary>
    public class Igralec
    {
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public string Pozicija { get; set; }

        // Readonly - igralec je "zaposlen" pri tocno doloceni ekipi,
        // ki je dolocena ob kreiranju.
        public Ekipa Ekipa { get; }

        public Igralec(string ime, string priimek, string pozicija, Ekipa ekipa)
        {
            Ime = ime;
            Priimek = priimek;
            Pozicija = pozicija;
            Ekipa = ekipa;
        }

        public override string ToString() => $"{Ime} {Priimek} ({Pozicija})";
    }
}
