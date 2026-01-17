using System.ComponentModel.DataAnnotations;

namespace UniObs.WebUI.Models
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "Doğrulama kodu zorunludur.")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Kod en az 4, en fazla 6 haneli olmalıdır.")]
        public string Code { get; set; }

        public string Message { get; set; }

        // Sayaç için kalan süre
        public int RemainingSeconds { get; set; } = 20;
    }
}
