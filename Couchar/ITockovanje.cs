namespace Couchar
{
    /// <summary>
    /// Vmesnik, ki ga implementira vsak sport. Definira, koliko tock
    /// (za potrebe vrstnega reda na tekmovanju) prinese posamezna
    /// kombinacija statistike "lastne" ekipe in statistike nasprotnika.
    /// S tem je metoda IzracunajVrstniRed v razredu Tekmovanje
    /// neodvisna od konkretnega sporta - sport sam pove, kako se tocke racunajo.
    /// </summary>
    public interface ITockovanje
    {
        int IzracunajTocke(Statistika lastnaStatistika, Statistika statistikaNasprotnika);
    }
}
