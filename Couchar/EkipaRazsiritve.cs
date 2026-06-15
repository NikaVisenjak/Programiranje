namespace Couchar
{
    /// <summary>
    /// Razred z razsiritveno metodo za Ekipa. Metoda IzpisiStatistiko
    /// zdruzi ToString ekipe in ToString podane statistike.
    /// </summary>
    public static class EkipaRazsiritve
    {
        public static string IzpisiStatistiko(this Ekipa ekipa, Statistika statistika)
        {
            return ekipa.ToString() + " - " + statistika.ToString();
        }
    }
}
