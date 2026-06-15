using KnjiznicaApp.Enumeracije;

namespace KnjiznicaApp.Modeli
{
    public class AvdioVizualnoGradivo : Gradivo
    {
        public VrstaAVGradiva Vrsta { get; set; }   // specifična lastnost 1
        public int TrajanjeMinut { get; set; }      // specifična lastnost 2

        public AvdioVizualnoGradivo(int id, string naslov, string avtor, int letoIzdaje,
                                      VrstaAVGradiva vrsta, int trajanjeMinut)
            : base(id, naslov, avtor, letoIzdaje)
        {
            Vrsta = vrsta;
            TrajanjeMinut = trajanjeMinut;
        }

        public override TipGradiva Tip => TipGradiva.AvdioVizualnoGradivo;

        public override string ToString()
            => base.ToString() + $" | Vrsta AV gradiva: {Vrsta}, Trajanje: {TrajanjeMinut} min";
    }
}
