using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineElection.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

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

        //[HttpGet]
        //public async Task<IActionResult> ReadPeople()
        //{
        //    var t = await appDbContext.People.FindAsync(id)
        //    return Json(t);
        //}
        [HttpGet("{id}")]
        public async Task<IActionResult>Read(long id)
        {
            var t = await appDbContext.People.FindAsync(id);
            return Json(t);
        }


        [HttpPut("{id}")]
        public   IActionResult Update(long id)
        {
            appDbContext.People.Update((appDbContext.People.FirstOrDefault(q => q.Id == id)));
            return  Ok();
        }

    }


    
}
