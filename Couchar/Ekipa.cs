using System.Collections.Generic;

namespace Couchar
{
    /// <summary>
    /// Abstraktni razred za ekipo. Konkretne ekipe so klub ali
    /// drzavna reprezentanca.
    /// </summary>
    public abstract class Ekipa
    {
        // Readonly - identifikacijski podatki ekipe se po kreiranju ne spremenijo.
        public string Ime { get; }
        public TipEkipe Tip { get; }

        public string Drzava { get; set; }

        // Readonly seznam - od zunaj je seznam igralcev viden samo za branje,
        // dodajanje pa je mozno le preko metode DodajIgralca.
        private readonly List<Igralec> igralci = new List<Igralec>();
        public IReadOnlyList<Igralec> Igralci => igralci;

        protected Ekipa(string ime, TipEkipe tip, string drzava)
        {
            Ime = ime;
            Tip = tip;
            Drzava = drzava;
        }

        public void DodajIgralca(Igralec igralec) => igralci.Add(igralec);

        public override string ToString() => $"{Ime} ({Drzava})";
    }
}
