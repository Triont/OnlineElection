using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineElection.ModelView;
using OnlineElection.Models;
using System.Text;
using OnlineElection.Services;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Newtonsoft.Json;

namespace OnlineElection.Controllers
{

    [Authorize]
    

    public class ElectionController : Controller
    {

        private readonly AppDbContext appDbContext;

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

        public async Task<IActionResult> CreateElect()
        {
            var tmp_user = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == tmp_user);
            if (user.EmailWasConfirmed)
            {

                return View();
            }
            else
            {
                return RedirectToAction("EmailNotConfirmed");
            }
        }

        public async Task<IActionResult> Elect(CreateElect createElect)
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
            var json_tmp = System.Text.Json.JsonSerializer.Serialize(voices);
            election.JSON_Election_Candidates += json_tmp;
            election.Status = "Actual";
            var Now = DateTime.Now;

            DateTime dateTime1 = DateTime.Now;
            double hours;
            double minutes;
            Double.TryParse(createElect.DurationHours, out hours);
            Double.TryParse(createElect.DurationMinutes, out minutes);
            var datetimeEnd = dateTime1.AddHours(hours).AddMinutes(minutes);
            //var hoursDuration=  TimeSpan.ParseExact(createElect.Duration, "HH", CultureInfo.InvariantCulture);
            //  var dateTime = Now.AddHours(hoursDuration.TotalHours);
            election.DateTimeEnd = datetimeEnd;
            await appDbContext.Elections.AddAsync(election);
            await appDbContext.SaveChangesAsync();
            //    await  Task.(int.Parse(createElect.Duration));
            //   Thread.Sleep(int.Parse(createElect.Duration));

            //   election.Status = "Archived";
            // appDbContext.Update(election);
            //await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }



        //[Authorize(""]
        public async Task<IActionResult> Show(long id)
        {
            var tmp_user = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == tmp_user);
            if (user.EmailWasConfirmed)
            {


                var _tmp = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == id);
                if (_tmp != null)
                {
                    var tmp_bool = System.Text.Json.JsonSerializer.Deserialize<Dictionary<long, bool>>(user.WasVotedId);
                    if (tmp_bool.ContainsKey(id))
                    {
                        if (tmp_bool[id])
                        {
                            TempData["AlreadyVote"] = true;
                            return RedirectToAction("AlreadyVoted!");
                        }

                    }
                    //var tmp = appDbContext.Elections.Where(i => i.Id == id).Select(i => i).FirstOrDefault();
                    Dictionary<string, long> t = (Dictionary<string, long>)System.Text.Json.JsonSerializer.Deserialize(_tmp.JSON_Election_Candidates,
                        typeof(Dictionary<string, long>));
                    //  List<Candidate> candidates = new List<Candidate>();
                    List<string> candidates = new List<string>();
                    ViewData["ElectName"] = _tmp.Name;
                    for (int i = 0; i < t.Count; i++)
                    {
                        candidates.Add(t.Keys.ElementAt(i));
                    }
                    ElectionView electionView = new ElectionView();
                    electionView.CandidatesElect = candidates;
                    electionView.Id = id;
                    electionView.Status = _tmp.Status;
                    return View(electionView);
                }


                Response.StatusCode = 404;
                return View($"~/Views/Shared/Error/{Response.StatusCode}.cshtml");

            }
            else
            {
                return RedirectToAction("EmailNotConfirmed");
            }
            // return View();

        }
        public IActionResult EmailNotConfirmed()
        {
            return View();
        }
        public IActionResult AlreadyVoted()
        {
            var check = (bool)TempData["AlreadyVote"];
            if (check)
            {
                return View();
            }
            else
            {
                return View("PageNotFound");
            }
        }



        public async Task<IActionResult> Result(ElectionView election)//refactored
        {
            long tempId = election.Id;
            Dictionary<string, long> VotesCount = new Dictionary<string, long>();
            Election CurElect = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == election.Id);

            var qwerty = User.FindFirstValue(ClaimTypes.Email);

            //var tqt = appDbContext.People.Where(i => i.Email == qwerty).Select(i => i).FirstOrDefault();
            var tqt = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == qwerty);
            if (tqt?.WasVotedId != null)
            {

                //  Dictionary<long, bool> keyValuePairs1 = new Dictionary<long, bool>();
                if (System.Text.Json.JsonSerializer.Deserialize(tqt?.WasVotedId, typeof(Dictionary<long, bool>)) is Dictionary<long, bool>)
                {
                    Dictionary<long, bool> values = (Dictionary<long, bool>)System.Text.Json.JsonSerializer.Deserialize(tqt?.WasVotedId, typeof(Dictionary<long, bool>));



                    if (!values.ContainsKey(election.Id))
                    {

                        values.Add(election.Id, true);
                        //   var t = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == election.Id);


                        VotesCount = (Dictionary<string, long>)System.Text.Json.JsonSerializer.Deserialize(CurElect?.JSON_Election_Candidates, typeof(Dictionary<string, long>));

                        VotesCount[election?.Selected]++;
                        string tmp_json = System.Text.Json.JsonSerializer.Serialize(VotesCount);
                        CurElect.JSON_Election_Candidates = tmp_json;

                        appDbContext.Elections.Update(CurElect);
                        string tmp_json_p = System.Text.Json.JsonSerializer.Serialize(values);
                        tqt.WasVotedId = tmp_json_p;
                        appDbContext.People.Update(tqt);
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
                string str_json = System.Text.Json.JsonSerializer.Serialize(keys);
                tqt.WasVotedId = str_json;
                appDbContext.People.Update(tqt);


                VotesCount = (Dictionary<string, long>)System.Text.Json.JsonSerializer.Deserialize(CurElect.JSON_Election_Candidates, typeof(Dictionary<string, long>));

                VotesCount[election?.Selected]++;
                string tmp_json = System.Text.Json.JsonSerializer.Serialize(VotesCount);
                CurElect.JSON_Election_Candidates = tmp_json;
                appDbContext.Elections.Update(CurElect);
                await appDbContext.SaveChangesAsync();

            }
            //var faf=     userManager.GetUserAsync(HttpContext.User);

            //     faf.Ema

            //    if(election.CandidatesElect.Count>1)
            //    {
            //        return RedirectToAction("Show");
            //    }
            //    string fst = election.Selected;
            //  var tmp=    appDbContext.Elections.Where(i => i.Id == election.Id).Select(i => i).FirstOrDefault();
            //    Dictionary<string, long> deserial = JsonSerializer.Deserialize(tmp.JSON_Election_Candidates,
            //        typeof(Dictionary<string, long>)) as Dictionary<string, long>;

            //    deserial[fst]++;
            //    var newJSON = JsonSerializer.Serialize(deserial);
            //    //var newJSON = JSONService.ToJSON(deserial);
            //    tmp.JSON_Election_Candidates = newJSON;
            //    appDbContext.Elections.Update(tmp);
            // appDbContext.SaveChanges();

            var winner_votes = VotesCount.Values.OrderByDescending(i => i).Select(k => k);
            var Name_Ien = VotesCount.OrderByDescending(i => i.Value).Select(i => i.Key);
            var winner = Name_Ien.FirstOrDefault();

            var votes_list = winner_votes.ToList();
            var name_list = Name_Ien.ToList();
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

            check_t.Add(CurElect.Id, true);
            var str__json = System.Text.Json.JsonSerializer.Serialize(check_t);
            tqt.WasVotedId = str__json;
            appDbContext.People.Update(tqt);
            await appDbContext.SaveChangesAsync();
            ResultView resultView = new ResultView();
            resultView.Names = name_list;
            resultView.Votes = votes_list;
            resultView.Percents = percents;

            resultView.id = CurElect.Id;
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

        //public async Task<IActionResult> ResultAsync(ElectionView electionView)
        //{
        //    var CurUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        //    var CurUser = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == CurUserEmail);
        //    Dictionary<long, bool> ElectedId = new Dictionary<long, bool>();
        //    if(CurUser.WasVotedId!=null)
        //    {
        //        ElectedId = (Dictionary<long, bool>)JsonSerializer.Deserialize(CurUser.WasVotedId, typeof(Dictionary<long, bool>));

        //    }
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Result(long id)
        {
            var elect = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == id);
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


                //   return Json(resultView);
                return View(resultView);
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResultJsonAjax(long id)
        {
            var elect = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == id);
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

                ViewData["Id"] = resultView.id;
                JsonView json = new JsonView() { json = JsonConvert.SerializeObject(resultView) };
                return View(json);
                //return View(resultView);
            }
            else
            {
                return View();
            }
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResultJson(long Id)
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

                ViewData["Id"] = resultView.id;
                JsonView json = new JsonView() { json = JsonConvert.SerializeObject(resultView) };
                var result=Json(resultView);
                return result;


                //return View(resultView);
            }
            else
            {
                return View();
            }
        }

      

    }
}
