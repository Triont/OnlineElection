using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineElection.ModelView;
using System.Text;
using Microsoft.Extensions.Logging;
using OnlineElection.Models;
using System.Security.Cryptography;
using OnlineElection.Services;

using System.Diagnostics;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace OnlineElection.Controllers
{
    public class User : Controller
    {
        // GET: User

        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext appDbContext;


        public User(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            this.appDbContext = appDbContext;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUser crUser)
        {
            if (ModelState.IsValid)
            {
                if (crUser.Pass != crUser.AgainPass)
                {
                    ViewData["Error"] = "Passwords do not the same";
                    return View();
                }
                if (appDbContext.People.Where(i => i.Email == crUser.Email).Count() != 0)
                {
                    ViewData["Error"] = "Email is already used";
                    return View();
                }

                Person person = new Person
                {
                    FirstName = crUser.FirstName,
                    SecondName = crUser.SecondName,
                    ThirdName = crUser.ThirdName,
                    Email = crUser.Email,
                    _DateTime = crUser._DateTime
                };

                var in_pass = crUser.Pass;

                var Rdgn = RandomNumberGenerator.Create();
                byte[] temparr = new byte[128];
                Rdgn.GetNonZeroBytes(temparr);
                byte[] temp =  Encoding.ASCII.GetBytes(in_pass);
                SHA512 sha512 = SHA512.Create();
                for (int i = 0; i < 10000; i++)
                {
                    var lx = sha512.ComputeHash(temp);

                    var t_res = lx.ToList().Concat(temparr);
                    temp = sha512.ComputeHash(t_res.ToArray());
                }
                person.Pass = Convert.ToBase64String(temp);
                person.Salt = Convert.ToBase64String(temparr);

                if(appDbContext.People.Where(i=>i==person).Count()==0)
                {
                    appDbContext.People.Add(person);
                    appDbContext.SaveChanges();
                }

                try
                {
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5

        public async Task< IActionResult> Login(LoginUser loginUser)
        {
            if (loginUser.Email!=null && loginUser.Password!=null)
            {
                var p = appDbContext.People.Where(i => i.Email == loginUser.Email).FirstOrDefault();
                if (p != null)
                {
                    var temp_pass = loginUser.Password;
                    var tmp_s = Convert.FromBase64String(p.Salt);
                    var tmp_res = HashSevice.GetHashStr(temp_pass, tmp_s, 10000);
                    if (tmp_res == p.Pass)
                    {
                        //if(p.WasVotedId!=null)
                        //{
                        await Authenticate(loginUser.Email, p.WasVotedId); // аутентификация

                        TempData["Name"] = loginUser.Email;
                        return RedirectToAction("Index", "Home");


                        //Dictionary<long, bool> tmp_v = new Dictionary<long, bool>();
                        //    tmp_v.Add(0, false);
                        //   var jsom_dictionary= System.Text.Json.JsonSerializer.Serialize(tmp_v);
                        //    await Authenticate(loginUser.Email, jsom_dictionary);
                        //    TempData["Name"] = loginUser.Email;
                        //    return RedirectToAction("Index", "Home");
                        
                     
                    }
                    else
                    {
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    }
                }
            }
            return View(loginUser);
        }

        private async Task Authenticate(string userName, string d)
        {

            //  var tmp=(Dictionary<long, bool>) System.Text.Json.JsonSerializer.Deserialize(d, typeof(Dictionary<long, bool>));
            // создаем один claim
            List<Claim> claims;
            if (d != null)
            {
                 claims = new List<Claim>
            {

                new Claim(ClaimTypes.Email, userName),

                new Claim("Voted elections", d),

            };
            }
            else
            {
                claims = new List<Claim>
            {

                new Claim(ClaimTypes.Email, userName),

               

            };
            }
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        [Authorize]
        public ActionResult CreateElect()
        {
            return RedirectToAction("CreateElect", "Election");
        }


        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
