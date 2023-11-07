using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using MimeKit;

namespace SakeFigureShop.Services
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class MailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        private readonly ILogger<MailService> _logger;

        public MailService(ILogger<MailService> logger)
        {
            _mailSettings = new MailSettings() {
                Mail = "quocanh944@gmail.com",
                DisplayName = "Sake Figure Shop",
                Password = "zgew zntu yhoa vgbw",
                Host = "smtp.gmail.com",
                Port = 587
            };
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(message);
            } catch (Exception ex)
            {
                _logger.LogInformation("Lỗi gửi email");
                _logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);
        }
    }

   
}
