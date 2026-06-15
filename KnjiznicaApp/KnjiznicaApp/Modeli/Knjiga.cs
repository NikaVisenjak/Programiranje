using KnjiznicaApp.Enumeracije;

namespace KnjiznicaApp.Modeli
{
    public class Knjiga : Gradivo
    {
        public VrstaKnjige Vrsta { get; set; }   // specifična lastnost 1
        public int SteviloStrani { get; set; }   // specifična lastnost 2

        public Knjiga(int id, string naslov, string avtor, int letoIzdaje,
                       VrstaKnjige vrsta, int steviloStrani)
            : base(id, naslov, avtor, letoIzdaje)
        {
            Vrsta = vrsta;
            SteviloStrani = steviloStrani;
        }

        public override TipGradiva Tip => TipGradiva.Knjiga;

        public override string ToString()
            => base.ToString() + $" | Vrsta knjige: {Vrsta}, Strani: {SteviloStrani}";
    }
}
