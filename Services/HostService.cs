using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Microsoft.AspNetCore.Html;
using MailKit.Net.Smtp;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using OnlineElection.Models;

namespace OnlineElection.Services
{

    public class ServiceT : IHostedService, IDisposable
    {

  
        private readonly ILogger<ServiceT> _log;
        private Timer _timer;
        private readonly IServiceScopeFactory serviceScopeFactory;
        //    private readonly AppData appData;
       // private readonly AppDbContext appDbContext;
        public ServiceT( IServiceScopeFactory serviceScopeFactory, ILogger<ServiceT> logger)
        {

            _log = logger;
            //  this.appData = appData;
            this.serviceScopeFactory = serviceScopeFactory;
       //     this._log = log;
         //   this.appDbContext = appDbContext;
        }
        //public void DoWork()
        //{


        //  //  if(appData.Items.FirstOrDefaultAsync(i=>i.Id==testS.Id[0]).Result.Time
        //}

        public void Dispose()
        {
            _timer.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("RecureHostedService is Starting");
            //List<Timer> timers = new List<Timer>();
            //AppData appData;
            //appData.Items.AsParallel().ForAll((i) => { timers.Add(i)})

          
            //    using (var scope = serviceScopeFactory.CreateScope())
            //    {
            //        var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //        //for (int i = 0; i < testS.Id.Count; i++)
            //        //{

            //        //    var t = await dbcontext.Items.FirstOrDefaultAsync(o => o.Id == testS.Id[i]);
            //        //    if (t?.DateTimeEnd.CompareTo(DateTime.Now)>0)
            //        //    {
            //        //Item item = new Item()
            //        //{
            //        //    UserCreatedId = 999,
            //        //    BeginPrice = 29,
            //        //    Name = "Test"
            //        //};
            //        //dbcontext.Items.Add(item);
            //        var qqqqq = dbcontext.Elections.AsNoTracking();

            //        List<Election> elections = new List<Election>();
            //    while (true)
            //    {

            //        await qqqqq.ForEachAsync((async i =>
            //        {
            //            await Task.Run(async() =>
            //            {
            //                if (DateTime.Now.CompareTo(i.DateTimeEnd) > 0)
            //                {
            //                    var s = i.JSON_Election_Candidates;
            //                    var deser = JsonSerializer.Deserialize<Dictionary<string, long>>(s);
            //                    var winner_votes = deser.Values.OrderByDescending(i => i).Select(k => k);
            //                    var Ien_names = deser.OrderByDescending(i => i.Value).Select(i => i.Key);
            //                    var winner = Ien_names.FirstOrDefault();
            //                    i.Status = "Archived";
            //                    i.Result = $"{winner} won";

            //                    dbcontext.Elections.Update(i);
            //                    await dbcontext.SaveChangesAsync();
            //                    //  elections.Add(i);
            //                    //      dbcontext.Update(i);
            //                    //     dbcontext.SaveChangesAsync();
            //                }
            //            });



            //            #region codefromTestRazor
            //            //await Task.Run(async () =>
            //            //{
            //            //    if (DateTime.Now.CompareTo(i.DateTimeEnd) >= 0)
            //            //    {
            //            //        i.Status = "Expired";
            //            //        if (i.BetWasDone && !i.ItemWasRedempt)
            //            //        {
            //            //            var buyer = await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == i.LastBetUserId);
            //            //            var seller = await dbcontext.Users.FirstOrDefaultAsync
            //            //            (q =>
            //            //                JSONConvert<List<long>>.
            //            //               GetIdListFromJSONString(q.ItemsList).Contains(i.Id)

            //            //           );
            //            //            if (seller != null)
            //            //            {
            //            //                await EmailSendService.SendEmailAsync(seller.EmailAddress, "WebAuction", $"Your item {i.Id} was ordered by {i.LastBetUserId}. Contact with " +
            //            //                    $"him to detail order, email {buyer.EmailAddress}" +
            //            //                    $"phone number {buyer.PhoneNumber}");
            //            //            }


            //            //            //var q=await dbcontext.Users.FirstOrDefaultAsync(i=>i.)
            //            //            //EmailSendService.SendEmailAsync()

            //            //            //  var u = await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == i.LastBetUserId);

            //            //            //sonSerializer.Serialize()
            //            //        }
            //            //        //   dbcontext.Items.Update(i);
            //            //        //         await    dbcontext.SaveChangesAsync();
            //            //    }
            //            //});
            //            #endregion
            //        }));


            //        await Task.Run(async () =>
            //         await Task.Run(() => {
            //             foreach (var i in qqqqq)
            //             {
            //                 dbcontext.Elections.Update(i);
            //            //   await dbcontext.SaveChangesAsync();
            //        }
            //         }));
            //        await dbcontext.SaveChangesAsync();
            //        //     dbcontext.Items.Remove(t);
            //        //   dbcontext.Items.Update(t);
            //        //await dbcontext.SaveChangesAsync();
            //        _log.LogInformation("Update from background service ");
            //        var t = dbcontext.Elections.ToList();
            //        //foreach (var i in t)
            //        //{
            //        //    _log.LogInformation($"{i.Status}");
            //        //}
            //        ////    }
            //        ////}
            //        //_log.LogInformation("Timed Background Service is working.");
            //    }
              
            //       await Task.Delay(5);
            //}
           _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
         //   return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("RecureHostedService is Stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private async void DoWork(object state)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                //for (int i = 0; i < testS.Id.Count; i++)
                //{

                //    var t = await dbcontext.Items.FirstOrDefaultAsync(o => o.Id == testS.Id[i]);
                //    if (t?.DateTimeEnd.CompareTo(DateTime.Now)>0)
                //    {
                //Item item = new Item()
                //{
                //    UserCreatedId = 999,
                //    BeginPrice = 29,
                //    Name = "Test"
                //};
                //dbcontext.Items.Add(item);
                var qqqqq = dbcontext.Elections.AsNoTracking();
                var test_qq = dbcontext.Elections.ToList();
                var temp = dbcontext.Elections.ToListAsync();
                foreach(var rrr in await temp)
                {
                   //try it
                }

                List<Election> elections = new List<Election>();
               

                 test_qq.ForEach(( i =>
                {
                     Task.Run(async() =>
                    {
                        if (DateTime.Now.CompareTo(i.DateTimeEnd) > 0)
                        {
                            var s = i.JSON_Election_Candidates;
                            var deser = JsonSerializer.Deserialize<Dictionary<string, long>>(s);
                            var winner_votes = deser.Values.OrderByDescending(i => i).Select(k => k);
                            var Ien_names = deser.OrderByDescending(i => i.Value).Select(i => i.Key);
                            var winner = Ien_names.FirstOrDefault();
                            i.Status = "Archived";
                            i.Result = $"{winner} won";
                             // elections.Add(i);
                                  dbcontext.Update(i);
                            //await    dbcontext.SaveChangesAsync();
                        }
                    });

               //     await dbcontext.SaveChangesAsync();

                    #region codefromTestRazor
                    //await Task.Run(async () =>
                    //{
                    //    if (DateTime.Now.CompareTo(i.DateTimeEnd) >= 0)
                    //    {
                    //        i.Status = "Expired";
                    //        if (i.BetWasDone && !i.ItemWasRedempt)
                    //        {
                    //            var buyer = await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == i.LastBetUserId);
                    //            var seller = await dbcontext.Users.FirstOrDefaultAsync
                    //            (q =>
                    //                JSONConvert<List<long>>.
                    //               GetIdListFromJSONString(q.ItemsList).Contains(i.Id)

                    //           );
                    //            if (seller != null)
                    //            {
                    //                await EmailSendService.SendEmailAsync(seller.EmailAddress, "WebAuction", $"Your item {i.Id} was ordered by {i.LastBetUserId}. Contact with " +
                    //                    $"him to detail order, email {buyer.EmailAddress}" +
                    //                    $"phone number {buyer.PhoneNumber}");
                    //            }


                    //            //var q=await dbcontext.Users.FirstOrDefaultAsync(i=>i.)
                    //            //EmailSendService.SendEmailAsync()

                    //            //  var u = await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == i.LastBetUserId);

                    //            //sonSerializer.Serialize()
                    //        }
                    //        //   dbcontext.Items.Update(i);
                    //        //         await    dbcontext.SaveChangesAsync();
                    //    }
                    //});
                    #endregion
                }));


                //await Task.Run(async () =>
                // await Task.Run(() =>
                // {
                //     foreach (var i in qqqqq)
                //     {
                //         dbcontext.Elections.Update(i);
                //        //   await dbcontext.SaveChangesAsync();
                //    }
                // }));
                //await dbcontext.SaveChangesAsync();
                //     dbcontext.Items.Remove(t);
                //   dbcontext.Items.Update(t);
                await dbcontext.SaveChangesAsync();
                _log.LogInformation("Update from background service ");
                var t = dbcontext.Elections.ToList();
                foreach (var i in t)
                {
                    _log.LogInformation($"{i.Status}");
                }
                //    }
                //}
                _log.LogInformation("Timed Background Service is working.");
            }
        }
    }
}
