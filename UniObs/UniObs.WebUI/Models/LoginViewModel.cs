using System.ComponentModel.DataAnnotations;

namespace UniObs.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }

        // ❗ Burada [Required] OLMAYACAK
        public string Message { get; set; }
    }
}
