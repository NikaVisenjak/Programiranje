namespace KnjiznicaApp.Enumeracije
{
    // Splošen tip gradiva - uporabljen v Knjiznica.IzpisiGradiva in v GradivoFactory
    public enum TipGradiva
    {
        Knjiga,
        PeriodicnaPublikacija,
        AvdioVizualnoGradivo
    }

    // Specifične "podvrste" znotraj razreda Knjiga
    public enum VrstaKnjige
    {
        Roman,
        Leksikon,
        Slikanica
    }

    // Specifične "podvrste" znotraj razreda PeriodicnaPublikacija
    public enum VrstaPublikacije
    {
        Casopis,
        Revija
    }

    // Specifične "podvrste" znotraj razreda AvdioVizualnoGradivo
    public enum VrstaAVGradiva
    {
        Vinilka,
        DVD
    }
}
