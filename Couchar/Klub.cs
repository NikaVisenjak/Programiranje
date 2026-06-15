namespace Couchar
{
    public class Klub : Ekipa
    {
        public string Stadion { get; set; }
        public string Sponzor { get; set; }

        public Klub(string ime, string drzava, string stadion, string sponzor)
            : base(ime, TipEkipe.Klub, drzava)
        {
            Stadion = stadion;
            Sponzor = sponzor;
        }
    }
}
