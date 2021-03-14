using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineElection.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using OnlineElection.ModelView;
using Microsoft.EntityFrameworkCore;

namespace OnlineElection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            this.appDbContext = appDbContext;
        }
        //public void PY()
        //{
        //    ScriptEngine engine = Python.CreateEngine();
        //    engine.Execute("print 'hello, world'");
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (!appDbContext.People.Contains(person))
            {
                appDbContext.People.Add(person);
                appDbContext.SaveChanges();
                return Ok();

            }

            return Conflict();
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> Read(long id)
        {
            var t = await appDbContext.People.FindAsync(id);
            return Json(t);
        }


        [HttpPut("{id}")]
        public IActionResult Update(long id)
        {
            appDbContext.People.Update((appDbContext.People.FirstOrDefault(q => q.Id == id)));
            return Ok();
        }

        public async Task<IActionResult> Search(FoundElect foundElect)
        {
            long test = 0;

            List<Election> elections = new List<Election>();
            if (long.TryParse(foundElect.Name, out test))
            {

                var qq = await appDbContext.Elections.FirstOrDefaultAsync(i => i.Id == test);
                if (qq != null)
                {
                    elections.Add(qq);
                }

            }
            else
            {
                if (foundElect.Name != null)
                {


                    if (await appDbContext.Elections.FirstOrDefaultAsync(i => i.Name.Contains(foundElect.Name)) != null)
                    {
                        var el = await appDbContext.Elections.Where(i => i.Name.Contains(foundElect.Name)).ToListAsync();
                        foreach (var i in el)
                        {
                            elections.Add(i);
                        }
                    }

                    else if (await appDbContext.Elections.FirstOrDefaultAsync(i => i.JSON_Election_Candidates.Contains(foundElect.Name)) != null)
                    {
                        var tq = await appDbContext.Elections.Where(i => i.JSON_Election_Candidates.Contains(foundElect.Name)).ToListAsync();
                        foreach (var q in tq)
                        {
                            elections.Add(q);
                        }

                    }
                }

            }
                return View(elections);


        }



    }
}
