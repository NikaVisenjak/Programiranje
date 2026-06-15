namespace BolnisnicaApp.Models
{
    /// <summary>
    /// Pomožni razred, ki predstavlja eno delovno nalogo.
    /// Uporablja se pri implementaciji vmesnika IZadolzen.
    /// </summary>
    public class Naloga
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public Zaposleni Zadolzeni { get; set; }
        public bool Zakljucena { get; set; }

        public Naloga(int id, string opis, Zaposleni zadolzeni)
        {
            Id = id;
            Opis = opis;
            Zadolzeni = zadolzeni;
            Zakljucena = false;
        }

        public override string ToString()
        {
            string status = Zakljucena ? "zaključena" : "v izvajanju";
            return $"#{Id}: {Opis} (zadolžen: {Zadolzeni}, status: {status})";
        }
    }
}
