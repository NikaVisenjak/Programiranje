namespace Couchar
{
    public class DrzavnaReprezentanca : Ekipa
    {
        public string Selektor { get; set; }

        public DrzavnaReprezentanca(string ime, string drzava, string selektor)
            : base(ime, TipEkipe.DrzavnaReprezentanca, drzava)
        {
            Selektor = selektor;
        }
    }
}
