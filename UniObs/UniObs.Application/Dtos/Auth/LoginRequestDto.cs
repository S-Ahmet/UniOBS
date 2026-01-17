namespace UniObs.Application.Dtos.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }  // log için IP bilgisini geçeceğiz
    }
}
