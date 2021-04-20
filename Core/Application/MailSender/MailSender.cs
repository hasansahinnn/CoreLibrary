using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.MailSender
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
        Task ConfirmationEmail(HttpContext context, Guid memberId, string email, string activationToken, string password, string firstname, string lastname);
        Task ForgotPasswordMail(HttpContext context, string email, string password, string name);
    }

    public class Mailer : IMailer
    {
        private readonly SmtpSettings _smtpSettings;

        public Mailer(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("",email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port);


                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
        public async Task ConfirmationEmail(HttpContext context, Guid memberId, string email, string activationToken, string password, string firstname, string lastname)
        {
            // Send an email with this link  
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(memberAsUser);
            var callbackUrl = $"http://{context.Request.Host}/api/users/ConfirmRegistration/{activationToken}";
            //Email from Email Template  
            string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
            // string body;  
            //Get TemplateFile located at wwwroot/Resources/Confirm_Mail_Template.html
            var pathToFile = Directory.GetCurrentDirectory()
                    + Path.DirectorySeparatorChar.ToString()
                    + "Resources"
                    + Path.DirectorySeparatorChar.ToString()
                    + "Confirm_Mail_Template.html";
            var subject = "Confirm Account Registration";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            string fullname = firstname + " " + lastname;
            string messageBody = string.Format(builder.HtmlBody,
                  password,
                  callbackUrl,
                  fullname
                  );
            await SendEmailAsync(email, subject, messageBody);
        }
        public async Task ForgotPasswordMail(HttpContext context, string email, string password, string name)
        {
            //Get TemplateFile located at wwwroot/Resources/Forgot_Password_Template.html
            var pathToFile = Directory.GetCurrentDirectory()
                    + Path.DirectorySeparatorChar.ToString()
                    + "Resources"
                    + Path.DirectorySeparatorChar.ToString()
                    + "Forgot_Password_Template.html";
            var subject = "Forgot Password";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            string messageBody = string.Format(builder.HtmlBody,
                  password,
                  name
                  );
            await SendEmailAsync(email, subject, messageBody);
        }

   
    }
 
}
