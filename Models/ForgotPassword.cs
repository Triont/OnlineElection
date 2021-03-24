using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.Models
{
    public class ForgotPassword
    {
        public long Id { get; set; }
        public string ResetToken { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public  long UserId { get; set; }
    }
}
