using System;
using KnjiznicaApp.Enumeracije;
using KnjiznicaApp.Modeli;

namespace KnjiznicaApp.Tovarna
{
    public static class GradivoFactory
    {
        // Glede na podan TipGradiva ustvari ustrezno konkretno instanco
        // gradiva. Parametra "specVrsta" in "specStevilo" predstavljata
        // specifični lastnosti posameznega tipa gradiva (npr. VrstaKnjige
        // in število strani, VrstaPublikacije in številko izdaje,
        // VrstaAVGradiva in trajanje v minutah).
        public static Gradivo UstvariGradivo(TipGradiva tip, int id, string naslov,
            string avtor, int letoIzdaje, object specVrsta, int specStevilo)
        {
            switch (tip)
            {
                case TipGradiva.Knjiga:
                    return new Knjiga(id, naslov, avtor, letoIzdaje,
                        (VrstaKnjige)specVrsta, specStevilo);

                case TipGradiva.PeriodicnaPublikacija:
                    return new PeriodicnaPublikacija(id, naslov, avtor, letoIzdaje,
                        (VrstaPublikacije)specVrsta, specStevilo);

                case TipGradiva.AvdioVizualnoGradivo:
                    return new AvdioVizualnoGradivo(id, naslov, avtor, letoIzdaje,
                        (VrstaAVGradiva)specVrsta, specStevilo);

                default:
                    throw new ArgumentException("Neznan tip gradiva.");
            }
        }
    }
}
