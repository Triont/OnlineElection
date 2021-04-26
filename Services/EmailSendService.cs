using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using System.IO;
using MimeKit;
using OnlineElection.Models;
using Microsoft.Extensions.Logging;

namespace OnlineElection.Services
{

    public interface ITokenGenerator
    {
        Task<string> Token(Person person, params object[] t);
    }

  public interface ISendAsync
    {
       
        Task<bool> SendAsync();
    }
    public interface ISendEmailAsync:ISendAsync
    {
   public   string Subject { get;  set; }
        public string EmailTo { get; set; }
        public string Message { get; set; }
        void SetData(string emailTo, string subject, string message);
       

    }
    [Obsolete]
    public class EmailSendService
    {

       
    // private static ILogger Logger { get; set; }
        public static async Task<string> SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            var build = new HtmlContentBuilder();
        //    build.AppendFormat($"<html><a href>{message ")
            emailMessage.From.Add(new MailboxAddress("Admin", "new.test.user.newtest@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<html><body><a href='{message}'>{message}</a></body></html>"

            };
            

            using (var client = new SmtpClient())
            {
                string pass = String.Empty;
                try
                {
                     pass = File.ReadAllText(@"C:\source\Test\mail_test.txt");
                }
                catch(DirectoryNotFoundException ex)
                {
                  //  Logger.LogCritical($"{ex.Message}, {ex.Data}, {ex.Source}");
                    return await Task.Run(() => $"Directory not found \t {ex.Message}, {ex.Data}, {ex.Source}");
                    
                }
                catch(FileNotFoundException ex)
                {
                  //  Logger.LogCritical($"{ex.Message}, {ex.Data}, {ex.Source}");
                    return await Task.Run(() => $"File not found \t {ex.Message}, {ex.Data}, {ex.Source} ");
                }
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("new.test.user.newtest@gmail.com", pass);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
                return await Task.Run(() => "Email was send");
            }
        }
        public async Task<string> Token(Person  user)
        {
            SHA512 sHA512_f = SHA512.Create();
            SHA512 sHA512_s = SHA512.Create();
            SHA512 sHA512_i = SHA512.Create();
            SHA512 sHA512_e = SHA512.Create();

          var r1=  Convert.ToBase64String(sHA512_f.ComputeHash(Encoding.ASCII.GetBytes(user.FirstName)));

            var r2 = Convert.ToBase64String(sHA512_s.ComputeHash(Encoding.ASCII.GetBytes(user.SecondName)));


            var r3 = Convert.ToBase64String(sHA512_i.ComputeHash(Encoding.ASCII.GetBytes(user.Id.ToString())));

            var r4 = Convert.ToBase64String(sHA512_e.ComputeHash(Encoding.ASCII.GetBytes(user.Email)));


            StringBuilder temp =new StringBuilder( Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            StringBuilder temp_q = new StringBuilder(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            temp.Append(r1);
            temp.Append(r2);
            temp.Append(temp_q);
            temp.Append(r3);
            temp.Append(r4);
         //   temp.Replace("+", "%2B");//Url encode

            // string token = "ConfirmToken";

            return await Task.Run(()=>temp.ToString());

        }

 
    }

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
    public class TokenGenerator:ITokenGenerator
    {
      public   async Task<string> Token(Person user, params object[] adds )
        {
            SHA512 sHA512_f = SHA512.Create();
            SHA512 sHA512_s = SHA512.Create();
            SHA512 sHA512_i = SHA512.Create();
            SHA512 sHA512_e = SHA512.Create();

            var r1 = Convert.ToBase64String(sHA512_f.ComputeHash(Encoding.ASCII.GetBytes(user.FirstName)));

            var r2 = Convert.ToBase64String(sHA512_s.ComputeHash(Encoding.ASCII.GetBytes(user.SecondName)));


            var r3 = Convert.ToBase64String(sHA512_i.ComputeHash(Encoding.ASCII.GetBytes(user.Id.ToString())));

            var r4 = Convert.ToBase64String(sHA512_e.ComputeHash(Encoding.ASCII.GetBytes(user.Email)));


            StringBuilder temp = new(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            StringBuilder temp_q = new(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            temp.Append(r1);
            temp.Append(r2);
            temp.Append(temp_q);
            temp.Append(r3);
            temp.Append(r4);
         

            return await Task.Run(() => temp.ToString());

        }
    }
}
