using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;


namespace OnlineElection.Services
{
    public interface IHash
    {
        string GetHashStr(string input, int iter_count);
        string GetHashStr(string input, byte[] salt, int iter_count);
        string GetHashStr(string input,  int iter_count, out string salt);


    }
  
    public class HashSevice:IHash
    {

        public  string GetHashStr(string input, int iter_count)
        {
            var Rdgn = RandomNumberGenerator.Create();
            byte[] temparr = new byte[128];
            Rdgn.GetNonZeroBytes(temparr);
            byte[] temp = Encoding.ASCII.GetBytes(input);
            SHA512 sha512 = SHA512.Create();
            for (int i = 0; i < iter_count; i++)
            {
                var lx = sha512.ComputeHash(temp);

                var t_res = lx.ToList().Concat(temparr);
                temp = sha512.ComputeHash(t_res.ToArray());
            }
            return Convert.ToBase64String(temp);
        }
        public  string GetHashStr(string input, int iter_count, out string salt)
        {
            var Rdgn = RandomNumberGenerator.Create();
            byte[] temparr = new byte[128];
            Rdgn.GetNonZeroBytes(temparr);
            byte[] temp = Encoding.ASCII.GetBytes(input);
            SHA512 sha512 = SHA512.Create();
            for (int i = 0; i < iter_count; i++)
            {
                var lx = sha512.ComputeHash(temp);

                var t_res = lx.ToList().Concat(temparr);
              
                temp = sha512.ComputeHash(t_res.ToArray());
            }
            salt = Convert.ToBase64String(temparr);
            return Convert.ToBase64String(temp);
        }
        public  string GetHashStr(string input, byte[] salt, int iter_count)
        {
            byte[] temp = Encoding.ASCII.GetBytes(input);
       //     byte[] _salt = Encoding.ASCII.GetBytes(salt);
            SHA512 sha512 = SHA512.Create();
            for (int i = 0; i < iter_count; i++)
            {
                var lx = sha512.ComputeHash(temp);

                var t_res = lx.ToList().Concat(salt);
                temp = sha512.ComputeHash(t_res.ToArray());
            }
            return Convert.ToBase64String(temp);
        }
    }
}
