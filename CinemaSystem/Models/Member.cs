namespace CinemaSystem.Models
{
    // =====================================================================
    // (6) Razred Member (registriran obiskovalec)
    // =====================================================================
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Dodatna lastnost - npr. točke zvestobe
        public int LoyaltyPoints { get; set; }
    }
}
