using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using OnlineElection.Models;

namespace OnlineElection.Services
{
    public class EmailSendService: IUserTwoFactorTokenProvider<OnlineElection.Models.Person>
    {
        
        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "new.test.user.newtest@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("new.test.user.newtest@gmail.com", "xw6MBTfKU2Hd");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        Task<bool> IUserTwoFactorTokenProvider<Person>.CanGenerateTwoFactorTokenAsync(UserManager<Person> manager, Person user)
        {
            throw new NotImplementedException();
        }

        Task<string> IUserTwoFactorTokenProvider<Person>.GenerateAsync(string purpose, UserManager<Person> manager, Person user)
        {
            string str = user.FirstName;
        var t=    System.Text.Encoding.ASCII.GetBytes(str);
            SHA512 sHA512 = SHA512.Create();
            for (int i = 0; i < 10000; i++)
            {
                t=sHA512.ComputeHash(t);
            }
            return Task.Run(() => str);

        }

       async Task<bool> IUserTwoFactorTokenProvider<Person>.ValidateAsync(string purpose, string token, UserManager<Person> manager, Person user)
        {
            throw new NotImplementedException();
        }
    }
}
