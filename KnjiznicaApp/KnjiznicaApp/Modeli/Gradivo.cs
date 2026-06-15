using System;
using KnjiznicaApp.Enumeracije;
using KnjiznicaApp.Vmesniki;

namespace KnjiznicaApp.Modeli
{
    public abstract class Gradivo : IIzposojljivo
    {
        // Skupne lastnosti vseh gradiv
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Avtor { get; set; }
        public int LetoIzdaje { get; set; }

        public bool JeRezervirano { get; set; }
        public bool JeIzposojeno { get; set; }

        // Vsak podrazred pove, kateremu tipu (enumeraciji) pripada -
        // uporabljeno v Knjiznica.IzpisiGradiva
        public abstract TipGradiva Tip { get; }

        protected Gradivo(int id, string naslov, string avtor, int letoIzdaje)
        {
            Id = id;
            Naslov = naslov;
            Avtor = avtor;
            LetoIzdaje = letoIzdaje;
            JeRezervirano = false;
            JeIzposojeno = false;
        }

        public virtual void Rezerviraj()
        {
            if (JeIzposojeno)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' je že izposojeno in ga ni mogoče rezervirati.");
            if (JeRezervirano)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' je že rezervirano.");

            JeRezervirano = true;
        }

        public virtual void Izposodi()
        {
            if (JeIzposojeno)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' je že izposojeno.");

            JeIzposojeno = true;
            JeRezervirano = false; // z izposojo morebitna rezervacija ugasne
        }

        public virtual void Vrni()
        {
            if (!JeIzposojeno)
                throw new InvalidOperationException(
                    $"Gradivo '{Naslov}' ni izposojeno, zato ga ni mogoče vrniti.");

            JeIzposojeno = false;
        }

        public override string ToString()
        {
            string stanje = JeIzposojeno ? "izposojeno"
                          : JeRezervirano ? "rezervirano"
                          : "na voljo";

            return $"[{Id}] {Naslov} ({Avtor}, {LetoIzdaje}) - {Tip} - {stanje}";
        }
    }
}
