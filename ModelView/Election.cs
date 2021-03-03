using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.ModelView
{
    public class ElectionView
    {
        public long Id { get; set; }
        public List<string> CandidatesElect { get; set; } = new List<string>();
        public string Selected { get; set; }
    }
}
