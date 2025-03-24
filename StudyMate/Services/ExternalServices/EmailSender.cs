
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyMate.Models;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigration emailConfigration;
        public EmailSender(EmailConfigration _emailConfigration)
        {
            emailConfigration = _emailConfigration;
            EnsureTemplatesFolderExists();

        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);

        }

     

        public async Task CreateAccountConfirmationEmail(AppUser user, string ConfirmationCode)
        {
              
            var content = (await GetEmailTemplateAsync("AccountConfirmationTemplate"))
                .Replace("{FirstName}", user.FirstName)
                .Replace("{ConfirmationCode}", ConfirmationCode);
             
            
          

            var message = new Message(new[] { user.Email }, "Account Confirmation for StudyMate", content, user.FirstName);            
            
            SendEmail(message);
        }

        public async Task CreatePasswordConfirmationEmail(AppUser user, string ConfirmationCode)
        {
            
            var content = (await GetEmailTemplateAsync("PasswordConfirmationTemplate"))
                .Replace("{FirstName}", user.FirstName)
                .Replace("{ConfirmationCode}", ConfirmationCode);
          
       
            var message = new Message(new[] { user.Email }, "Password Reset", content, user.FirstName, null);    
            SendEmail(message);
        }

        public async Task<string> GetEmailTemplateAsync(string templateName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates",
                $"{templateName}.html");
            return await File.ReadAllTextAsync(filePath);
        }

        private void Send(MimeMessage emailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(emailConfigration.SmtpServer, emailConfigration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(emailConfigration.UserName, emailConfigration.Password);
                    client.Send(emailMessage);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("From StudyMate", emailConfigration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Content
            };
            foreach (var attachment in message.Attachments)
            {
                bodyBuilder.Attachments.Add(attachment);
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
        private void EnsureTemplatesFolderExists()
        {
            var templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
            if (!Directory.Exists(templatesPath))
            {
                Directory.CreateDirectory(templatesPath);
            }
        }
    }
}
