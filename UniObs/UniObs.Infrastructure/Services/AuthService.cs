using UniObs.Application.Contracts.Repositories;
using UniObs.Application.Contracts.Services;
using UniObs.Application.Dtos.Auth;
using BCryptNet = BCrypt.Net.BCrypt; // alias

namespace UniObs.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IYoneticiRepository _yoneticiRepository;
        private readonly IEmailService _emailService;

        private const int MaxFailedAccessCount = 3;       // Hatalı giriş limiti
        private const int LockoutSeconds = 20;            // Hesap kilit süresi
        private const int TwoFactorCodeLifetimeSeconds = 20;  // OTP geçerlilik süresi

        public AuthService(IYoneticiRepository yoneticiRepository, IEmailService emailService)
        {
            _yoneticiRepository = yoneticiRepository;
            _emailService = emailService;
        }

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto request)
        {
            var result = new LoginResultDto();
            var user = await _yoneticiRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                result.Success = false;
                result.Message = "Kullanıcı adı veya şifre hatalı.";
                return result;
            }

            // 💥 Kilit kontrolü
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
            {
                var kalan = (int)(user.LockoutEnd.Value - DateTime.Now).TotalSeconds;
                result.Success = false;
                result.IsLockedOut = true;
                result.Message = $"Hesabınız {kalan} saniye boyunca kilitli.";
                return result;
            }

            // 🔥 BCrypt + düz metin geriye dönük destek
            bool sifreDogru = false;

            if (!string.IsNullOrEmpty(user.Sifre) && user.Sifre.StartsWith("$2"))
            {
                sifreDogru = BCryptNet.Verify(request.Password, user.Sifre);
            }
            else
            {
                if (request.Password == user.Sifre)
                {
                    sifreDogru = true;

                    // ilk girişte hashle
                    user.Sifre = BCryptNet.HashPassword(request.Password);
                    await _yoneticiRepository.UpdateAsync(user);
                }
            }

            if (!sifreDogru)
            {
                user.FailedAccessCount++;

                if (user.FailedAccessCount >= MaxFailedAccessCount)
                {
                    user.LockoutEnd = DateTime.Now.AddSeconds(LockoutSeconds);
                    await _yoneticiRepository.UpdateAsync(user);

                    result.Success = false;
                    result.IsLockedOut = true;
                    result.Message =
                        $"Çok sayıda hatalı giriş nedeniyle hesabınız {LockoutSeconds} saniye kilitlenmiştir.";
                    return result;
                }

                await _yoneticiRepository.UpdateAsync(user);

                result.Success = false;
                result.Message = "Kullanıcı adı veya şifre hatalı.";
                return result;
            }

            // login başarılı → sıfırla
            user.FailedAccessCount = 0;
            user.LockoutEnd = null;

            // OTP üret
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            user.TwoFactorCode = code;
            user.TwoFactorExpiresAt = DateTime.Now.AddSeconds(TwoFactorCodeLifetimeSeconds);

            await _yoneticiRepository.UpdateAsync(user);

            await _emailService.SendEmailAsync(
                user.Email,
                "OBS Doğrulama Kodu",
                $"Doğrulama kodunuz: {code}\nKod {TwoFactorCodeLifetimeSeconds} saniye geçerlidir."
            );

            result.Success = true;
            result.RequiresTwoFactor = true;
            result.Email = user.Email;
            result.Role = user.Role;
            result.Message = "Doğrulama kodu e-postanıza gönderildi.";

            return result;
        }

        public async Task<LoginResultDto> VerifyTwoFactorCodeAsync(string email, string code)
        {
            var result = new LoginResultDto();
            var user = await _yoneticiRepository.GetByEmailAsync(email);

            if (user == null)
            {
                result.Success = false;
                result.Message = "Kullanıcı bulunamadı.";
                return result;
            }

            if (string.IsNullOrEmpty(user.TwoFactorCode) ||
                !user.TwoFactorExpiresAt.HasValue ||
                user.TwoFactorExpiresAt < DateTime.Now ||
                !string.Equals(user.TwoFactorCode, code))
            {
                result.Success = false;
                result.Message = "Doğrulama kodu hatalı veya süresi dolmuş.";
                return result;
            }

            user.TwoFactorCode = null;
            user.TwoFactorExpiresAt = null;
            await _yoneticiRepository.UpdateAsync(user);

            result.Success = true;
            result.Email = user.Email;
            result.Role = user.Role;
            result.Message = "Giriş başarılı.";

            return result;
        }

        public async Task<LoginResultDto> ResendTwoFactorCodeAsync(string email)
        {
            var result = new LoginResultDto();
            var user = await _yoneticiRepository.GetByEmailAsync(email);

            if (user == null)
            {
                result.Success = false;
                result.Message = "Kullanıcı bulunamadı.";
                return result;
            }

            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            user.TwoFactorCode = code;
            user.TwoFactorExpiresAt = DateTime.Now.AddSeconds(TwoFactorCodeLifetimeSeconds);

            await _yoneticiRepository.UpdateAsync(user);

            await _emailService.SendEmailAsync(
                user.Email,
                "Yeni Doğrulama Kodu",
                $"Yeni doğrulama kodunuz: {code}\nKod {TwoFactorCodeLifetimeSeconds} saniye geçerlidir."
            );

            result.Success = true;
            return result;
        }
    }
}
