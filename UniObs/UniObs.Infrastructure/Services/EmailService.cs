using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using UniObs.Application.Contracts.Services;

namespace UniObs.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"] ?? "true");
            var email = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"];

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(email, password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(email!, "Üniversite OBS"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
}
