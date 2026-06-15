using System.Collections.Generic;

namespace CinemaSystem.Models
{
    // =====================================================================
    // Razred CineCompany (podjetje)
    // - Name    -> avtomatsko implementirana lastnost
    // - Cinemas -> ne-avtomatsko implementirana lastnost (zasebno polje + dostopnik)
    // - Members -> avtomatsko implementirana lastnost (Dictionary za hitro iskanje)
    // =====================================================================
    public class CineCompany
    {
        public string Name { get; set; }

        // Ne-avtomatsko implementirana lastnost
        private List<Cinema> _cinemas = new List<Cinema>();
        public List<Cinema> Cinemas => _cinemas;

        // (6) Registrirani obiskovalci shranjeni v slovarju (Dictionary<int, Member>),
        // kjer je ključ ID člana.
        //
        // Izbrana podatkovna struktura: Dictionary
        // Razlog: iskanje, dodajanje in posodabljanje člana po ID-ju je v povprečju O(1),
        // medtem ko bi iskanje v navadnem List<Member> zahtevalo O(n) - linearni pregled.
        // Ker pri velikem številu članov pričakujemo pogosto iskanje po ID-ju
        // (npr. ob nakupu vstopnice, prijavi v sistem...), je Dictionary
        // bistveno učinkovitejša izbira.
        public Dictionary<int, Member> Members { get; set; } = new Dictionary<int, Member>();

        public CineCompany(string name)
        {
            Name = name;
        }

        public void AddCinema(Cinema cinema) => _cinemas.Add(cinema);

        public void AddMember(Member member) => Members[member.Id] = member;
    }
}
