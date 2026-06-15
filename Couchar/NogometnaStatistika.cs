namespace Couchar
{
    /// <summary>Statistika ekipe pri nogometu.</summary>
    public class NogometnaStatistika : Statistika
    {
        public int Goli { get; set; }
        public int RumeniKartoni { get; set; }
        public int RdeciKartoni { get; set; }
        public double PosestZoge { get; set; } // v odstotkih [0-100]

        public NogometnaStatistika(Tekma tekma) : base(tekma) { }

        public override string ToString()
        {
            return $"Goli: {Goli}, Rumeni kartoni: {RumeniKartoni}, " +
                   $"Rdeci kartoni: {RdeciKartoni}, Posest zoge: {PosestZoge:F1}%";
        }
    }
}
