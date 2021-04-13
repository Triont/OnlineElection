using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineElection.Models;
using OnlineElection.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace OnlineElection.Controllers
{
   
   
    [ApiController] 
    [Produces("application/json")]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public ValuesController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        [HttpGet]
        [AllowAnonymous]
       [Route("Get/{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
           
            var elect = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == Id);
            if (elect != null)
            {


                //or create Result table 

                //    var tmp = appDbContext.Elections.Where(i => i.Id == election.Id).Select(i => i).FirstOrDefault();
                Dictionary<string, long> deserial = System.Text.Json.JsonSerializer.Deserialize(elect.JSON_Election_Candidates,
                    typeof(Dictionary<string, long>)) as Dictionary<string, long>;

                //    deserial[fst]++;
                //   var newJSON = JsonSerializer.Serialize(deserial);
                //var newJSON = JSONService.ToJSON(deserial);
                //tmp.JSON_Election_Candidates = newJSON;
                //appDbContext.Elections.Update(tmp);
                //appDbContext.SaveChanges();

                var winner_votes = deserial.Values.OrderByDescending(i => i).Select(k => k);
                var nameQueue = deserial.OrderByDescending(i => i.Value).Select(i => i.Key);
                var winner = nameQueue.FirstOrDefault();

                var votes_list = winner_votes.ToList();
                var name_list = nameQueue.ToList();
                long sum = 0;
                for (int i = 0; i < votes_list.Count; i++)
                {
                    sum += votes_list[i];
                }
                List<double> percents = new List<double>();
                for (int i = 0; i < votes_list.Count; i++)
                {
                    double tm = ((double)votes_list[i] / (double)sum) * 100.0;
                    double _tm = Math.Round(tm, 2);
                    percents.Add(_tm);
                }
                Dictionary<long, bool> check_t = new Dictionary<long, bool>();

                check_t.Add(elect.Id, true);
                var str__json = System.Text.Json.JsonSerializer.Serialize(check_t);
                //rwrf.WasVotedId = str__json;
                //appDbContext.People.Update(rwrf);
                //appDbContext.SaveChanges();
                ResultView resultView = new ResultView();
                resultView.Names = name_list;
                resultView.Votes = votes_list;
                resultView.Percents = percents;
                resultView.id = elect.Id;
                if (elect.Status == "Actual")
                {
                    resultView.Active = true;
                }
                else
                {
                    resultView.Active = false;
                }
                for (int i = 0; i < resultView.Percents.Count; i++)
                {
                    if (resultView.Percents.Count == resultView.Votes.Count)
                    {
                        if (Double.IsNaN(resultView.Percents[i]) || double.IsInfinity(resultView.Percents[i]))
                        {
                            resultView.Percents[i] = 0;
                        }
                        if (Double.IsNaN(resultView.Votes[i]) || double.IsInfinity(resultView.Votes[i]))
                        {
                            resultView.Votes[i] = 0;
                        }
                    }

                }

                //ViewData["Id"] = resultView.id;
                //JsonView json = new JsonView() { json = JsonConvert.SerializeObject(resultView) };
                return Ok(resultView);


                //return View(resultView);
            }
            //else
            //{
            //    return View();
            //}
            else
            {
                return BadRequest();
            }
        }

    }
}
