using Demo.DAL.Models;
using System;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public class EmailSettings
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }

        public void SendEmail(Email email)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                throw new InvalidOperationException("EmailSettings chưa được cấu hình. Hãy chạy: dotnet user-secrets set \"EmailSettings:UserName\" \"<gmail>\" và \"EmailSettings:Password\" \"<app-password>\" trong project Demo.PL.");

            using var client = new SmtpClient(Host, Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(UserName, Password)
            };

            client.Send(FromAddress ?? UserName, email.Recipients, email.Subject, email.Body);
        }
    }
}
