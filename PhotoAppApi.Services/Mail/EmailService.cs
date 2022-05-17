using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Mail
{
    public class EmailService
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _senderSmtpServer;

        public EmailService(IConfiguration config)
        {
            var emailsSernderSection = config.GetSection("EmailSender");
            _senderEmail = emailsSernderSection["Email"];
            _senderPassword = emailsSernderSection["EmailPassword"];
            _senderSmtpServer = emailsSernderSection["SmtpServer"];
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _senderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                await client.ConnectAsync(_senderSmtpServer, 465, true);
                await client.AuthenticateAsync(_senderEmail, _senderPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public string CreateHash(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

        
            String chars = "[^a-zA-Z]";
            string str = Convert.ToBase64String(valueBytes);

            return Regex.Replace(str, chars, String.Empty);
        }

        public  bool ValidateHash(string value, string salt, string hash)
            => CreateHash(value, salt) == hash;
    }
}
