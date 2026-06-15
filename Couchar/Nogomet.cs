namespace Couchar
{
    public class Nogomet : Sport
    {
        public Nogomet() : base("Nogomet", VrstaSporta.Nogomet) { }

        public override Statistika UstvariStatistiko(Tekma tekma) => new NogometnaStatistika(tekma);

        public override int IzracunajTocke(Statistika lastna, Statistika nasprotnika)
        {
            var l = (NogometnaStatistika)lastna;
            var n = (NogometnaStatistika)nasprotnika;

            if (l.Goli > n.Goli) return 3; // zmaga
            if (l.Goli == n.Goli) return 1; // neodloceno
            return 0;                       // poraz
        }
    }
}
