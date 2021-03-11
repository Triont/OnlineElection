using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.Models
{
    public class ConfirmToken
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public  string Email { get; set; }

        public string Token { get; set; }
        public DateTime CreationDateTime { get; set; }
      
        public int LifeTimeMin { get; set; }
    }
}
