namespace KnjiznicaApp.Vmesniki
{
    // Vmesnik določa lastnosti/vedenje vseh objektov, ki jih je mogoče
    // izposoditi oziroma rezervirati. Implementira ga abstraktni razred
    // Gradivo, lahko pa bi ga implementiral tudi povsem drugačen razred,
    // ki nima nič skupnega z gradivom (npr. razred Kanu za izposojo
    // kanuja ob Soči).
    public interface IIzposojljivo
    {
        bool JeRezervirano { get; set; }
        bool JeIzposojeno { get; set; }

        void Rezerviraj();
        void Izposodi();
        void Vrni();
    }
}
