using MailKit.Net.Smtp;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using static NotificationsApi.Service.SendNotificationService;
using MailKit.Security;


namespace NotificationsApi.Service
{
    public class SendNotificationService : ISendNotificationService
    {
        private readonly IConfiguration _config;

        public SendNotificationService(IConfiguration config)
        {

            _config = config;
        }

        public async Task SendEmailVerifyEamilAsync(string toEmail, string userName, string token)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Reste your password";
            string resetLink = $"http://localhost:3000/reset-password?token={Uri.EscapeDataString(token)}";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $@"Hello {userName},

                    Tap the link below to reset your LS account password:

                   {resetLink}

                    If you did not request this, you can ignore this email.
                    "

            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }

        public async Task SendWelcomeEmailAsync(string toEmail, string userName, string role)
            {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "🎉 Welcome to Lasophy";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $"Hello {userName},\n\nYour account with role '{role}' has been created.\nWelcome aboard!"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        
    }
}
