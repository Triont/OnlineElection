using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.Models
{
    public class Candidate
    {
        public long Id { get; set; }

        
        public long ElectionId { get; set; }
        public string Name { get; set; }
        public long CountVotes { get; set; }
    }
}
