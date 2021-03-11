using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Html;
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
            var build = new HtmlContentBuilder();
        //    build.AppendFormat($"<html><a href>{message ")
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "new.test.user.newtest@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<html><body><a href='{message}'>{message}</a></body></html>"

            };
            

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("new.test.user.newtest@gmail.com", "xw6MBTfKU2Hd");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
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
      var str1=      temp.Append(r1);
     var str2=       str1.Append(r2);
         var str3=   str2.Append(temp_q);
          var str4=str3.Append(r3);
       var str5=     str4.Append(r4);
         //  var r= str5.Replace("+", "%2B");

           // string token = "ConfirmToken";
            
            return await Task.Run(()=>str5.ToString());

        }

        Task<bool> IUserTwoFactorTokenProvider<Person>.CanGenerateTwoFactorTokenAsync(UserManager<Person> manager, Person user)
        {
            throw new NotImplementedException();
        }

        Task<string> IUserTwoFactorTokenProvider<Person>.GenerateAsync(string purpose, UserManager<Person> manager, Person user)
        {
          string token=Convert.ToBase64String(  Guid.NewGuid().ToByteArray());

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
