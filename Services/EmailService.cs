using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Html;
using System.IO;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace OnlineElection.Services
{
    public class EmailService : ISendEmailAsync
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }
        public string EmailTo { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public void SetData(string EmailTo, string Subject, string Message)
        {
            this.EmailTo = EmailTo;
            this.Message = Message;
            this.Subject = Subject;
        }
        public async Task<bool> SendAsync()
        {
            if ((EmailTo != null) && (Message != null) && (Subject != null))
            {

                var emailMessage = new MimeMessage();
                var build = new HtmlContentBuilder();
                //    build.AppendFormat($"<html><a href>{message ")
                emailMessage.From.Add(new MailboxAddress("Admin", "new.test.user.newtest@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", EmailTo));
                emailMessage.Subject = Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $"<html><body><a href='{Message}'>{Message}</a></body></html>"

                };


                using var client = new SmtpClient();
                string pass = String.Empty;
                try
                {
                    pass = File.ReadAllText(@"C:\source\Test\mail_test.txt");
                }
                catch (DirectoryNotFoundException ex)
                {
                     logger.LogCritical($"Directory not found \t{ ex.Message}, {ex.Data}, {ex.Source}");
                     await Task.Run(() => $"Directory not found \t {ex.Message}, {ex.Data}, {ex.Source}");
                    return false;

                }
                catch (FileNotFoundException ex)
                {
                    logger.LogCritical($"{ex.Message}, {ex.Data}, {ex.Source}");
                     await Task.Run(() => $"File not found \t {ex.Message}, {ex.Data}, {ex.Source} ");
                    return false;
                }
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("new.test.user.newtest@gmail.com", pass);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
                await Task.Run(() => "Email was send");
                return await Task.Run(() => true);

            }
            else
            {
                logger.LogCritical($"No data to email");
                
                return false;
            }

        }
    }
}
