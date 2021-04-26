using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OnlineElection.Models;

namespace OnlineElection.Services
{
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
