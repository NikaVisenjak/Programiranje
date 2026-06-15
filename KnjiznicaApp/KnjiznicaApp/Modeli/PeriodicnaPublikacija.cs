using KnjiznicaApp.Enumeracije;

namespace KnjiznicaApp.Modeli
{
    public class PeriodicnaPublikacija : Gradivo
    {
        public VrstaPublikacije Vrsta { get; set; }   // specifična lastnost 1
        public int StevilkaIzdaje { get; set; }       // specifična lastnost 2

        public PeriodicnaPublikacija(int id, string naslov, string avtor, int letoIzdaje,
                                       VrstaPublikacije vrsta, int stevilkaIzdaje)
            : base(id, naslov, avtor, letoIzdaje)
        {
            Vrsta = vrsta;
            StevilkaIzdaje = stevilkaIzdaje;
        }

        public override TipGradiva Tip => TipGradiva.PeriodicnaPublikacija;

        public override string ToString()
            => base.ToString() + $" | Vrsta publikacije: {Vrsta}, Številka izdaje: {StevilkaIzdaje}";
    }
}
