namespace Couchar
{
    /// <summary>Statistika ekipe pri kosarki.</summary>
    public class KosarkarskaStatistika : Statistika
    {
        public int Koshi { get; set; }
        public int Asistence { get; set; }
        public int Skoki { get; set; }

        public KosarkarskaStatistika(Tekma tekma) : base(tekma) { }

        public override string ToString()
        {
            return $"Koshi: {Koshi}, Asistence: {Asistence}, Skoki: {Skoki}";
        }
    }
}
