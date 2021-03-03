﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElection.ModelView
{
    public class ResultView
    {
        public List<long> Votes { get; set; }
        public List<string> Names { get; set; }
        public List<double> Percents { get; set; }
    }
}