using System;

namespace BolnisnicaApp.Interfaces
{
    /// <summary>
    /// Vmesnik, ki ga implementirajo zaposleni, ki so lahko zadolženi za naloge.
    /// </summary>
    public interface IZadolzen
    {
        /// <summary>
        /// Zaposleni prevzame novo nalogo z danim opisom.
        /// </summary>
        void PrevzemiNalogo(string opis);

        /// <summary>
        /// Označi nalogo z danim ID-jem kot zaključeno.
        /// </summary>
        void ZakljuciNalogo(int idNaloge);

        /// <summary>
        /// Prenese nalogo z danim ID-jem na drugega zaposlenega.
        /// </summary>
        void PrenesiNalogo(int idNaloge, Zaposleni novZadolzeni);
    }
}
