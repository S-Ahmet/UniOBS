using System.Threading.Tasks;
using UniObs.Application.Dtos.Auth;

namespace UniObs.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto request);
        Task<LoginResultDto> VerifyTwoFactorCodeAsync(string email, string code);
        Task<LoginResultDto> ResendTwoFactorCodeAsync(string email);
    }
}
