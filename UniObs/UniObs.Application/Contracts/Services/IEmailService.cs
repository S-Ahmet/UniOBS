using System.Threading.Tasks;

namespace UniObs.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
