using MailKit.Net.Smtp;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using static NotificationsApi.Service.SendEmailService;
using MailKit.Security;


namespace NotificationsApi.Service
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _config;

        public SendEmailService(IConfiguration config)
        {

            _config = config;
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
