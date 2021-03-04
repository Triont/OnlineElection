using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.ModelView
{
    public class FoundElect
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> Candidates { get; set; }
    }
}
