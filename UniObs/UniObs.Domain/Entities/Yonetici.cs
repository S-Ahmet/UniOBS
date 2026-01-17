namespace UniObs.Domain.Entities
{
    public class Yonetici
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Sifre { get; set; }

        public int FailedAccessCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string Role { get; set; }

        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorExpiresAt { get; set; }
    }
}
