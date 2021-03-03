using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace OnlineElection.Models
{
    public class Election
    {
        
        //[Key]
        //public  long Id { get; set; }
        ////   readonly static List<string> Vs = new List<string>();
        //[NotMapped]
        //public Dictionary<string, long> Count { get; private set; } = new Dictionary<string, long>();
       
        //public string _Count { get
        //    {
        //        return JsonSerializer.Serialize(this.Count);
        //    } 
        //}
       
        //public string Name { get; set; }
        //public  string Result
        //{
        //    get
        //    {
                
        //      var tmp=  from entry in Count orderby Count?.Values descending select entry;
        //        return tmp.First().Key;
                
        //    }
        //}
        
        //public  void AddCandidate(string Name)
        //{
        //    Count.Add(Name, 0);
        //}
        //public  void Add(string Name)
        //{
        //    for(int i=0; i<Count.Count;i++)
        //    {
        //        if(Count.ElementAt(i).Key==Name)
        //        {
        //         Count[Name]=( Count[Name]++);
        //        }
        //    }
        //}

        public long Id { get; set; }
        public string Name { get; set; }

        public string JSON_Election_Candidates { get; set; }
        public long Count { get; set; }
        public string Result { get; set; }
    }
}
