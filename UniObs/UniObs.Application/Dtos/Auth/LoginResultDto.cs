namespace UniObs.Application.Dtos.Auth
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public string Email { get; set; }      // başarılı girişte dolu
        public bool IsLockedOut { get; set; }  // hesap kilitli mi

        public string Role { get; set; }

        public bool RequiresTwoFactor { get; set; }
    }
}
