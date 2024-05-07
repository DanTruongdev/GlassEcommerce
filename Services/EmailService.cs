using GlassECommerce.Services.Interfaces;
using GlassECommerce.Services.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace GlassECommerce.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public bool SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            return Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Notification System", _config["EmailConfiguration:From"]));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private bool Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            int port = int.Parse(_config["EmailConfiguration:Port"]);
            try
            {
                client.Connect(_config["EmailConfiguration:SmtpServer"], port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_config["EmailConfiguration:Username"], _config["EmailConfiguration:Password"]);
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

}

