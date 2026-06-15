using System;

namespace CinemaSystem.Models
{
    // =====================================================================
    // Pomožni razred - posamezen vnos v programu dvorane
    // (datum, ura, film, ki se takrat predvaja)
    // =====================================================================
    public class ProgramEntry
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Movie Movie { get; set; }

        // Pomožna izračunana lastnost - čas pričetka in konca predvajanja
        public DateTime StartDateTime => Date.Date + Time;
        public DateTime EndDateTime => StartDateTime + Movie.Duration;
    }
}
