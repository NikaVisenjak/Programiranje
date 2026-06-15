namespace Couchar
{
    public class Kosarka : Sport
    {
        public Kosarka() : base("Kosarka", VrstaSporta.Kosarka) { }

        public override Statistika UstvariStatistiko(Tekma tekma) => new KosarkarskaStatistika(tekma);

        public override int IzracunajTocke(Statistika lastna, Statistika nasprotnika)
        {
            var l = (KosarkarskaStatistika)lastna;
            var n = (KosarkarskaStatistika)nasprotnika;

            // V kosarki ni neodlocenih izidov.
            return l.Koshi > n.Koshi ? 2 : 1;
        }
    }
}
