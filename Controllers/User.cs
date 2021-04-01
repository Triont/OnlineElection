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
using System.Text.Json;

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
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View();
                }
            }
            else return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
           var email= User.FindFirst(ClaimTypes.Email)?.Value;
            var curUser = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == email);
            return View(curUser);
        }

        public IActionResult EditView()
        {
          string personJson = TempData["CurUser"] as string;
            EditViewModel person = JsonSerializer.Deserialize<EditViewModel>(personJson);
            return View(person);
        }

        // GET: User/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}
        public IActionResult PasswordPetry()
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditById(int id, IFormCollection collection)
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

        [HttpGet]
        public async Task<IActionResult> ForgotCheck()
        {
            if (Request.Query.ContainsKey("token"))
            {
                string value = Request.Query["token"];
                //      string value_check = value.Replace(' ', '+');
                var ql = appDbContext.FogrotPasswords.ToList();
                for (int i = 0; i < ql.Count; i++)
                {
                    if (ql[i].ResetToken == value)
                    {
                       
                        if (DateTime.Now.CompareTo(ql[i].DateTimeEnd) <= 0)
                        {
                            var tmp = await appDbContext.People.FirstOrDefaultAsync(u => u.Id == ql[i].UserId);
                            //tmp.EmailWasConfirmed = true;
                            //appDbContext.People.Update(tmp);
                            //await appDbContext.SaveChangesAsync();
                            TempData["UserId"] = ql[i].UserId.ToString();
                            return RedirectToAction("PassNew", "User");
                        }
                       

                    }
                }

            }
            return RedirectToAction("Index", "Home");

        }
        public IActionResult PassNew()
        {
            return View();
        }

        public async Task<IActionResult> PassChange(NewPass newPass)
        {
            var userChangePass = await appDbContext.People.FirstOrDefaultAsync(i => i.Id == long.Parse( newPass.UserId));
            if(userChangePass!=null)
            {
                string salt;
                var hashedPass = HashSevice.GetHashStr(newPass.Pass, 10000, out salt);
                userChangePass.Pass = hashedPass;
                userChangePass.Salt = salt;
                appDbContext.People.Update(userChangePass);
                await appDbContext.SaveChangesAsync();


            }
            return RedirectToAction("Login");
        }
        public IActionResult Forgotten()
        {
            return View();
        }

        public async Task<IActionResult> Forgot(ForgottenPass forgotten)
        {
            var user = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == forgotten.Email);
            if (user != null)
            {
               var token=await email.Token(user);
                StringBuilder path = new StringBuilder(Request.Scheme);
                path.Append("://");
                path.Append(Request.Host.Value);
                path.Append("/User/ForgotCheck");
                path.Append("/?token=");

                //  string s =Request.Scheme+"://"+ Request.Host.Value;
                ////  var allowedString = String.Concat(s.Select(i => i)) ;

                //  var tmp_res = s + "/User/ConfirmEmail";
                //  var ttt = tmp_res + "/?token=";
             //   var token = await email.Token(_person);
                StringBuilder stringBuilder = new StringBuilder(token);
                stringBuilder.Replace("+", "%2B");
                path.Append(stringBuilder.ToString());



                await appDbContext.FogrotPasswords.AddAsync(new ForgotPassword()
                {

                    DateTimeCreate = DateTime.Now,
                    //   Email = user.Email,

                    DateTimeEnd = DateTime.Now.AddMinutes(5),
                    ResetToken = token,
                    UserId = user.Id

                }) ;
                await appDbContext.SaveChangesAsync();

                var log_tmp = await EmailSendService.SendEmailAsync(user.Email, "Reset password", path.ToString());
                _logger.LogInformation(log_tmp);

                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");

        }
        [Authorize]
        public IActionResult PasswordRetry()
        {
        
            
            return View();
        }

        public async Task<IActionResult> CheckPassword(CheckPass checkPass)
        {
            var Email = User.FindFirst(ClaimTypes.Email)?.Value;
            var curUser = await appDbContext.People.FirstOrDefaultAsync(i => i.Email == Email);
            EditViewModel editViewModel = new EditViewModel();
            if(curUser!=null)
            {
              var tmp=  HashSevice.GetHashStr(checkPass.Pass, Convert.FromBase64String(curUser.Salt), 10000);
           
                if(tmp==curUser.Pass)
                {
                  //  TempData["User"]=curUser;
                    editViewModel.Id = curUser.Id;
                    editViewModel.FirstName = curUser.FirstName;
                    editViewModel.SecondName = curUser.SecondName;
                    editViewModel.ThirdName = curUser.ThirdName;
                    editViewModel.Email = curUser.Email;
                    TempData["CurUser"] = JsonSerializer.Serialize(editViewModel);
                    return RedirectToAction("EditView");
                }
                else
                {
                    return RedirectToAction("NotCorrect");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        
        public async Task<IActionResult> Edit(Person person)
        {

            var personToEdit = await appDbContext.People.FirstOrDefaultAsync(i => i.Id == person.Id);
            if(personToEdit!=null)
            {
                if(personToEdit.FirstName!=person.FirstName)
                {
                    personToEdit.FirstName = person.FirstName;
                }
                if(personToEdit.SecondName!=person.SecondName)
                {
                    personToEdit.SecondName = person.SecondName;
                }
                if(personToEdit.ThirdName!=person.ThirdName)
                {
                    personToEdit.ThirdName = person.ThirdName;
                }
                if(personToEdit.Email!=person.Email)
                {
                    personToEdit.Email = person.Email;
                    personToEdit.EmailWasConfirmed = false;
                }
                appDbContext.People.Update(personToEdit);
                await appDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        
        }

        private async Task Authenticate(string userName, string d)
        {

         
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
            // created object ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // set authentication cookies
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
       
            }
            return RedirectToAction("Index", "Home");
                }
        public async Task< IActionResult> ConfirmEmailCreate(Person _person)
        {
            string str = User.FindFirst(ClaimTypes.Email)?.Value;
            if(str!=null)
            {
              var q=await  appDbContext.People.FirstOrDefaultAsync(i => i.Email == str);
         

            }

            //more fast
            StringBuilder path = new StringBuilder(Request.Scheme);
            path.Append("://");
            path.Append(Request.Host.Value);
            path.Append("/User/ConfirmEmail");
            path.Append("/?token=");

          //  string s =Request.Scheme+"://"+ Request.Host.Value;
          ////  var allowedString = String.Concat(s.Select(i => i)) ;
        
          //  var tmp_res = s + "/User/ConfirmEmail";
          //  var ttt = tmp_res + "/?token=";
            var token =await email.Token(_person);
            StringBuilder stringBuilder = new StringBuilder(token);
            stringBuilder.Replace("+", "%2B");
            path.Append(stringBuilder.ToString());

    
         
           await appDbContext.ConfirmTokens.AddAsync(new ConfirmToken()
            {
                CreationDateTime = DateTime.Now,
                Email=_person.Email,
          
                LifeTimeMin = 10, Token=token,PersonId=_person.Id
               
            });
            await appDbContext.SaveChangesAsync();
      
          var log_tmp=  await EmailSendService.SendEmailAsync(_person.Email, "Confirm", path.ToString());
            _logger.LogInformation(log_tmp);
        
            return RedirectToAction("Login");
        }
    }
}
