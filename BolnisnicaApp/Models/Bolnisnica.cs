using System.Collections.Generic;

namespace BolnisnicaApp.Models
{
    /// <summary>
    /// Razred predstavlja bolnišnico - hrani seznam zaposlenih in seznam pacientov.
    /// </summary>
    public class Bolnisnica
    {
        // --- Samodejno implementirani lastnosti ---
        public string Naziv { get; set; }
        public List<Pacient> Pacienti { get; set; } = new List<Pacient>();

        // --- Ne-samodejno implementirana lastnost ---
        // Seznam zaposlenih ima zaseben backing field. "Set" del poskrbi,
        // da seznam ni nikoli null, dodajanje pa poteka prek metode
        // DodajZaposlenega, ki preprečuje podvajanje istega zaposlenega.
        private List<Zaposleni> seznamZaposlenih = new List<Zaposleni>();
        public List<Zaposleni> SeznamZaposlenih
        {
            get => seznamZaposlenih;
            set => seznamZaposlenih = value ?? new List<Zaposleni>();
        }

        public Bolnisnica(string naziv)
        {
            Naziv = naziv;
        }

        /// <summary>
        /// Doda zaposlenega v bolnišnico, če ta še ni v seznamu.
        /// </summary>
        public void DodajZaposlenega(Zaposleni zaposleni)
        {
            if (!seznamZaposlenih.Contains(zaposleni))
            {
                seznamZaposlenih.Add(zaposleni);
            }
        }

        public override string ToString()
        {
            return Naziv;
        }
    }
}
