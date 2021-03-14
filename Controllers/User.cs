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
using Microsoft.EntityFrameworkCore;

namespace OnlineElection.Controllers
{
    public class User : Controller
    {
        // GET: User

        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext appDbContext;
        private readonly EmailSendService email;

        public User(ILogger<HomeController> logger, AppDbContext appDbContext, EmailSendService email)
        {
            _logger = logger;
            this.appDbContext = appDbContext;
            this.email = email;
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
        public async Task< ActionResult> Create(CreateUser crUser)
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
                  await  appDbContext.People.AddAsync(person);
                   await appDbContext.SaveChangesAsync();
                    //     return RedirectToAction("ConfirmEmail", new { _person = person });
                 await   ConfirmEmailCreate(person);
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

    
     
        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login(LoginUser loginUser, string returnUrl)
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
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {

                        //    string st = "/";
                       
                          var t=  returnUrl.Split('/');
                           
                           
                            for(var i=0; i<t.Length;i++)
                            {
                              t[i]=  t[i].Replace('#', default(char));
                            }
                             var res=t.Reverse().Take(3);
                            return RedirectToAction(res.ElementAt(1), res.ElementAt(2), new { id = res.ElementAt(0) });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }


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


        // GET: User/Delete/5
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

        public IActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            if(Request.Query.ContainsKey("token"))
            {
                string value = Request.Query["token"];
          //      string value_check = value.Replace(' ', '+');
                var ql = appDbContext.ConfirmTokens.ToList();
                for(int i=0;i<ql.Count;i++)
                {
                    if (ql[i].Token==value)
                    {
                        TimeSpan timeSpan = new TimeSpan(0, ql[i].LifeTimeMin, 0);
                        var qw = ql[i].CreationDateTime + timeSpan;
                        if (DateTime.Compare(DateTime.Now, qw) <= 0)
                        {
                            var tmp = await appDbContext.People.FirstOrDefaultAsync(u => u.Id == ql[i].PersonId);
                            tmp.EmailWasConfirmed = true;
                            appDbContext.People.Update(tmp);
                            await appDbContext.SaveChangesAsync();
                        }
                        return RedirectToAction("Index", "Home");

                    }
                }
             //   var q = await appDbContext.ConfirmTokens.FirstOrDefaultAsync(i => i.Token == value);
                //if(q!=null )
                //{
                //    TimeSpan timeSpan = new TimeSpan(0, q.LifeTimeMin,0);
                //    var qw = q.CreationDateTime + timeSpan;
                //    if(DateTime.Compare(DateTime.Now, qw)<=0)
                //    {
                //        var tmp = await appDbContext.People.FirstOrDefaultAsync(i => i.Id == q.PersonId);
                //        tmp.EmailWasConfirmed = true;
                //        appDbContext.People.Update(tmp);
                //        await appDbContext.SaveChangesAsync();
                //    }
                //  //compare time
                //  //imp
                //  //

                //}
            }
            return RedirectToAction("Index", "Home");
                }
        public async Task< IActionResult> ConfirmEmailCreate(Person _person)
        {
            string str = User.FindFirst(ClaimTypes.Email)?.Value;
            if(str!=null)
            {
              var q=await  appDbContext.People.FirstOrDefaultAsync(i => i.Email == str);
                //if(q.EmailWasConfirmed)
                //{
                    
                //}

            }
            //  EmailSendService emailSendService = new EmailSendService();
            //var tq=   await emailSendService.Token(user);
            //  await EmailSendService.SendEmailAsync(user.Email, "Confirm", tq);
            
            string s =Request.Scheme+"://"+ Request.Host.Value;
            var allowedString = String.Concat(s.Select(i => i)) ;
            //var r = allowedString.Concat("/?token=");
            //string sq=new string(r.ToArray());
            //  var tmp_res = s + Request.Path;
            var tmp_res = s + "/User/ConfirmEmail";
            var ttt = tmp_res + "/?token=";
            var qqqq =await email.Token(_person);

    
            var res = ttt + qqqq;
           await appDbContext.ConfirmTokens.AddAsync(new ConfirmToken()
            {
                CreationDateTime = DateTime.Now,
                Email=_person.Email,
          
                LifeTimeMin = 10, Token=qqqq,PersonId=_person.Id
               
            });
            await appDbContext.SaveChangesAsync();
            //StringBuilder stringBuilder = new StringBuilder(res);
            //stringBuilder.Replace("+", "%2B");
          var log_tmp=  await EmailSendService.SendEmailAsync(_person.Email, "Confirm", res);
            _logger.LogInformation(log_tmp);
            //var tq=   await emailSendService.Token(user);
            //  await EmailSendService.SendEmailAsync(user.Email, "Confirm", tq);
            //  var allowedStringR = String.Concat(allowedString.Select(i => i));
            return RedirectToAction("Login");
        }
    }
}
