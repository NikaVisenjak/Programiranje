namespace Couchar
{
    /// <summary>
    /// Abstraktni razred, ki predstavlja statistiko ene ekipe na eni tekmi.
    /// Konkretne vsebine (goli, koshi, ...) so odvisne od sporta in jih
    /// definirajo podrazredi.
    /// </summary>
    public abstract class Statistika
    {
        // Readonly - tekma, na katero se statistika nanasa, je dolocena
        // ob kreiranju in se kasneje ne spremeni.
        public Tekma Tekma { get; }

        protected Statistika(Tekma tekma)
        {
            Tekma = tekma;
        }

        public abstract override string ToString();
    }
}
