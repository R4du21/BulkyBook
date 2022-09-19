using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailtoSend = new MimeMessage();
            emailtoSend.From.Add(MailboxAddress.Parse("raducuitp@gmail.com"));
            emailtoSend.To.Add(MailboxAddress.Parse(email));
            emailtoSend.Subject = subject;
            emailtoSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            //send the email
            using(var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("raducuitp@gmail.com", "bmlettsmalvoywkr");
                emailClient.Send(emailtoSend);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
