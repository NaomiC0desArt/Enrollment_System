
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.Interfaces.Auth;

namespace UniversitySystem.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Result> SendConfirmationEmailAsync(string email, string token)
        {
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var safeToken = Uri.EscapeDataString(token);

            var link = $"{baseUrl}/api/auth/confirm-email?token={safeToken}&email={email}";
            // 1. Build a MimeMessage
            var message = new MimeMessage();
            // 2. Set From, To, Subject
            message.From.Add(new MailboxAddress("system", _config["Email:From"]));
            message.To.Add(new MailboxAddress("user", email));

            message.Subject = "Confirm your account";
            // 3. Set the body — include the token in a link

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <h1>Welcome to the enrollment system!!</h1>
                <p>Please click the link below to confirm you account:</p>
                <a href='{link}'>Confirm My Account</a>
                <p>If the link doesn't work, copy and paste this: {link}"
                
            };

            message.Body = bodyBuilder.ToMessageBody();

            return await ExecuteSendAsync(message);
            
           
        }

        public async Task<Result> SendPasswordResetEmailAsync(string email, string token)
        {
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var safeToken = Uri.EscapeDataString(token);

            var link = $"{baseUrl}/api/auth/reset-password?token={safeToken}&email={email}";
            // 1. Build a MimeMessage
            var message = new MimeMessage();
            // 2. Set From, To, Subject
            message.From.Add(new MailboxAddress("system", _config["Email:From"]));
            message.To.Add(new MailboxAddress("user", email));

            message.Subject = "Password reset";
            // 3. Set the body — include the token in a link
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <p>You're receiving this e-mail because you requested a password reset for your account.</p>
                <p>Please click the folloring link to reset your password:</p>
                <a href='{link}'>Reset password</a>
                <p>If the link doesn't work, copy and paste this: {link}"

            };

            message.Body = bodyBuilder.ToMessageBody();

            return await ExecuteSendAsync(message);
        }

        private async Task<Result> ExecuteSendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            try
            {
                // 5. Connect to the host and port from config
                await client.ConnectAsync(_config["Email:Host"], _config.GetValue<int>("Email:Port"), SecureSocketOptions.None);
                // 6. Send the message
                await client.SendAsync(message);
                // 7. Disconnect
                await client.DisconnectAsync(true);
                return Result.Success();

            }
            catch (Exception ex)
            {
                return Result.Failure("Email could not be sent.");
            }
        }
    }
}
