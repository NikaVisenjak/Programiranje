using System;

namespace Couchar
{
    /// <summary>Pomozni razred, ki glede na izbiro uporabnika vrne ustrezno fabriko.</summary>
    public static class SportFactoryPonudnik
    {
        public static SportFactory PridobiFactory(VrstaSporta vrsta)
        {
            return vrsta switch
            {
                VrstaSporta.Nogomet => new NogometFactory(),
                VrstaSporta.Kosarka => new KosarkaFactory(),
                _ => throw new ArgumentException("Neznana vrsta sporta.")
            };
        }
    }
}
