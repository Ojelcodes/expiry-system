using Application.ConfigSettings;
using Application.DTOs;
using Application.InfraInterfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly EmailSettings _emailSettings;

        public NotificationService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendMail(EmailRequest emailRequest)
        {
             var client = new SmtpClient("smtp.gmail.com", 587)
             {
                 Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                 EnableSsl = true
             };

            MailMessage message = new(_emailSettings.Username, emailRequest.Email, emailRequest.Subject, emailRequest.Message);
            message.IsBodyHtml = true;
            client.Send(message);
        }
    }
}
