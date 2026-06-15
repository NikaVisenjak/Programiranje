using System;

namespace CinemaSystem.Interfaces
{
    // =====================================================================
    // (3) Vmesnik IProgram
    // =====================================================================
    public interface IProgram
    {
        // Vrne true, če je dvorana v podanem datumu/času/intervalu prosta
        bool IsEmpty(DateTime date, TimeSpan time, TimeSpan interval);
    }
}
