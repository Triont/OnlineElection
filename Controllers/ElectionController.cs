using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineElection.ModelView;
using OnlineElection.Models;
using System.Text;
using OnlineElection.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OnlineElection.Controllers
{
  
    public class ElectionController : Controller
    {
       // private readonly UserManager<IdentityUser> userManager;
        private readonly AppDbContext appDbContext;
        //private readonly Lazy<AppDbContext> dbContext;
        public JSONService<Dictionary<string, long>> JSONService;

        public ElectionController(AppDbContext appDbContext)
        {
        
            this.appDbContext = appDbContext;
           // dbContext = appDb;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateElect()
        {
            return View();
        }

        public IActionResult Elect(CreateElect createElect)
        {

            Models.Election election = new Models.Election
            {
                Name = createElect.Name
            };
            Dictionary<string, long> voices = new Dictionary<string, long>();
            if (createElect.CandidateFirst != null)
            {
                voices.Add(createElect.CandidateFirst, 0);
            }

            if (createElect.CandidateSecond != null)
            {
                voices.Add(createElect.CandidateSecond, 0);
            }
            if (createElect.CandidateThird != null)
            {
                voices.Add(createElect.CandidateThird, 0);
            }
            if (createElect.CandidateFourth != null)
            {
                voices.Add(createElect.CandidateFourth, 0);
            }
            if (createElect.CandidateFifth != null)
            {


                voices.Add(createElect.CandidateFifth, 0);
            }
            var json_tmp = JsonSerializer.Serialize(voices);
            election.JSON_Election_Candidates += json_tmp;
            appDbContext.Elections.Add(election);
            appDbContext.SaveChanges();

            //ModelView.ElectionView election1 = new ModelView.ElectionView();
            //for(int i=0;i<election.Count.Count;i++)
            //{
            //    election1._Value.Add(election.Count.Keys.ElementAt(i), false);
            //}
            //return View(election1);
            return RedirectToAction("Index", "Home");

        }

   
        public  IActionResult Show(long id)
        {

             var tmp=   appDbContext.Elections.Where(i => i.Id == id).Select(i => i).FirstOrDefault();
             Dictionary<string,long> t=(Dictionary<string, long>) JsonSerializer.Deserialize(tmp.JSON_Election_Candidates, 
                 typeof(Dictionary<string, long>));
            //  List<Candidate> candidates = new List<Candidate>();
            List<string> candidates = new List<string>();
            ViewData["ElectName"] = tmp.Name;
           for(int i=0; i< t.Count;i++)
            {
                candidates.Add(t.Keys.ElementAt(i));
            }
            ElectionView electionView = new ElectionView();
            electionView.CandidatesElect = candidates;
            electionView.Id = id;
            return View(electionView);
        }
        
        
        public async Task< IActionResult> Result(ElectionView election)
        {
            long tempId = election.Id;
            Dictionary<long, bool> keyValuePairs = new Dictionary<long, bool>();
            //HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        //    dbContext.Value.People.Where(i => i.Id == election.Id).Select(i => i).FirstOrDefault();
            var qwerty = User.FindFirstValue(ClaimTypes.Email);
            
            var rwrf = appDbContext.People.Where(i => i.Email == qwerty).Select(i => i).FirstOrDefault();
            var tqt =await appDbContext.People.FirstOrDefaultAsync(i => i.Email == qwerty);
           if(rwrf.WasVotedId!=null)
            {
                Dictionary<long, bool> keyValuePairs1 = new Dictionary<long, bool>();
                if (JsonSerializer.Deserialize(rwrf.WasVotedId, typeof(Dictionary<long, bool>)) is Dictionary<long, bool>)
                {
                    Dictionary<long, bool> values = (Dictionary<long, bool>)JsonSerializer.Deserialize(rwrf.WasVotedId, typeof(Dictionary<long, bool>));


                 
                    if (!values.ContainsKey(election.Id))
                    {

                        values.Add(election.Id, true);
                        var t = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == election.Id);

                        var Dict = (Dictionary<string, long>)JsonSerializer.Deserialize(t.JSON_Election_Candidates, typeof(Dictionary<string, long>));

                        Dict[election?.Selected]++;
                        string tmp_json = JsonSerializer.Serialize(Dict);
                        t.JSON_Election_Candidates = tmp_json;
                        appDbContext.Elections.Update(t);
                        string tmp_json_p = JsonSerializer.Serialize(values);
                        rwrf.WasVotedId = tmp_json_p;
                        appDbContext.People.Update(rwrf);
                        await appDbContext.SaveChangesAsync();
                        //return RedirectToAction("Result");
                    }
                    else if (values[election.Id] == true)
                    {
                        //ViewData["Notification"] = "You have already voted!";
                        //RedirectToPage()
                        return Redirect($"Show/{election.Id}");
                    }

                  






                }
            }
            else
            {
                Dictionary<long, bool> keys = new Dictionary<long, bool>();
                keys.Add(election.Id, true);
               string str_json= JsonSerializer.Serialize(keys);
                rwrf.WasVotedId = str_json;
                appDbContext.People.Update(rwrf);
                var t = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == election.Id);

                var Dict = (Dictionary<string, long>)JsonSerializer.Deserialize(t.JSON_Election_Candidates, typeof(Dictionary<string, long>));

                Dict[election?.Selected]++;
                string tmp_json = JsonSerializer.Serialize(Dict);
                t.JSON_Election_Candidates = tmp_json;
                appDbContext.Elections.Update(t);
                await appDbContext.SaveChangesAsync();

            }
       //var faf=     userManager.GetUserAsync(HttpContext.User);
            
       //     faf.Ema

            if(election.CandidatesElect.Count>1)
            {
                return RedirectToAction("Show");
            }
            string fst = election.Selected;
        var tmp=    appDbContext.Elections.Where(i => i.Id == election.Id).Select(i => i).FirstOrDefault();
            Dictionary<string, long> deserial = JsonSerializer.Deserialize(tmp.JSON_Election_Candidates,
                typeof(Dictionary<string, long>)) as Dictionary<string, long>;

            deserial[fst]++;
            var newJSON = JsonSerializer.Serialize(deserial);
            //var newJSON = JSONService.ToJSON(deserial);
            tmp.JSON_Election_Candidates = newJSON;
            appDbContext.Elections.Update(tmp);
         appDbContext.SaveChanges();

            var winner_votes = deserial.Values.OrderByDescending(i => i).Select(k=>k);
            var rfs = deserial.OrderByDescending(i => i.Value).Select(i => i.Key);
            var winner = rfs.FirstOrDefault();

            var votes_list = winner_votes.ToList();
            var name_list = rfs.ToList();
            long sum = 0;
            for(int i=0;i<votes_list.Count;i++)
            {
                sum += votes_list[i];
            }
            List<double> percents = new List<double>();
            for(int i=0;i<votes_list.Count;i++)
            {
                double tm =( (double)votes_list[i] /(double) sum)*100.0;
              double _tm= Math.Round(tm, 2);
                percents.Add(_tm);
            }
            Dictionary<long, bool> check_t = new Dictionary<long, bool>();

            check_t.Add(tmp.Id, true);
            var str__json = JsonSerializer.Serialize(check_t);
            rwrf.WasVotedId = str__json;
            appDbContext.People.Update(rwrf);
            appDbContext.SaveChanges();
            ResultView resultView = new ResultView();
            resultView.Names = name_list;
            resultView.Votes = votes_list;
            resultView.Percents = percents;
            return View(resultView);


         //   if(election._Value.Values.Where(i=>i==true).Count()>1)
         //   {
         //       ModelState.AddModelError("", "only one");
         //   }

         //string str=   election._Value.Where(i => i.Value == true).Select(i => i.Key).FirstOrDefault();

         //   try
         //   {
         //       var tmp = appDbContext.Elections.Where(i => i.Id == election.Id).FirstOrDefault();

         //       tmp.Add(str);
         //       long tmp_c = 0;
         //       for (int i = 0; i < tmp.Count.Count; i++)
         //       {
         //           tmp_c += (tmp.Count[tmp.Count.Keys.ElementAt(i)]);
         //       }
         //       appDbContext.Elections.Update(tmp);
         //       appDbContext.SaveChanges();

         //       ViewData["Voters"] = tmp_c;
         //       return View(tmp);
         //   }
         //   catch(NullReferenceException d)
         //   {
         //       return RedirectToAction("Elect", "Election");
         //   }

                
        }
    }
}
