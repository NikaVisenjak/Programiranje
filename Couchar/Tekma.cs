using System;

namespace Couchar
{
    public class Tekma
    {
        // Readonly - tekmovanje, na katerem je tekma odigrana, se ne spremeni.
        public Tekmovanje Tekmovanje { get; }

        // Readonly - ekipi, ki se pomerita, sta dolocene ob kreiranju tekme.
        public Ekipa Ekipa1 { get; }
        public Ekipa Ekipa2 { get; }

        public DateTime Datum { get; set; }

        // Statistiki obeh ekip - tip je odvisen od sporta tekmovanja
        // in se doloci avtomatsko ob kreiranju tekme.
        public Statistika Statistika1 { get; }
        public Statistika Statistika2 { get; }

        public Tekma(Tekmovanje tekmovanje, Ekipa ekipa1, Ekipa ekipa2, DateTime datum)
        {
            Tekmovanje = tekmovanje;
            Ekipa1 = ekipa1;
            Ekipa2 = ekipa2;
            Datum = datum;

            // Sport tekmovanja (preko Factory metode UstvariStatistiko)
            // dolocata, kaksen tip Statistika dobi vsaka ekipa.
            Statistika1 = tekmovanje.Sport.UstvariStatistiko(this);
            Statistika2 = tekmovanje.Sport.UstvariStatistiko(this);
        }

        public override string ToString() => $"{Ekipa1} - {Ekipa2} ({Datum:d})";
    }
}
