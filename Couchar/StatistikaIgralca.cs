namespace Couchar
{
    /// <summary>
    /// Statistika posameznega igralca - poseben razred, ki ni neposredno
    /// del hierarhije Statistika (ni nujno vezan na en sport), ampak
    /// se navezuje na konkretnega igralca.
    /// </summary>
    public class StatistikaIgralca
    {
        // Readonly - igralec, na katerega se statistika nanasa.
        public Igralec Igralec { get; }

        public int OdigraneMinute { get; set; }
        public int Goli { get; set; }
        public int Asistence { get; set; }

        public StatistikaIgralca(Igralec igralec)
        {
            Igralec = igralec;
        }

        public override string ToString()
        {
            return $"{Igralec}: {OdigraneMinute} min, {Goli} golov, {Asistence} asistenc";
        }
    }
}
