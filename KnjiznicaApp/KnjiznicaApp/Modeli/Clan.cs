using System.Collections.Generic;

namespace KnjiznicaApp.Modeli
{
    public class Clan
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Priimek { get; set; }

        public List<Gradivo> IzposojenoGradivo { get; set; }
        public List<Gradivo> VrnjenoGradivo { get; set; }

        public Clan(int id, string ime, string priimek)
        {
            Id = id;
            Ime = ime;
            Priimek = priimek;
            IzposojenoGradivo = new List<Gradivo>();
            VrnjenoGradivo = new List<Gradivo>();
        }

        public override string ToString() => $"{Ime} {Priimek} (ID {Id})";
    }
}
