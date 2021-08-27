using IpWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TweetSharp;
using System.Drawing;

namespace IpWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IpDbContext db = new IpDbContext();
        
        [Authorize]
        public ActionResult Index()
        {
            List<Client> temp = db.Client.ToList();
            temp.Reverse();
            List < Client > client = temp.Take(5).ToList();
            List<Record> temp2 = db.Record.ToList();
            temp2.Reverse();
            List<Record> Record = temp2.Take(5).ToList();
            ViewData["Records"] = Record;
            ViewData["Clients"] = client;
            return View(db.Task.ToList());
        }

        [Authorize]
        public ActionResult Settings(int? days)
        {
            if(days != null)
            {
                Models.Settings.DaysToAlertBeforeDeadline = days.Value;
            }
            return View();
        }

        [Authorize]
        public ActionResult supportUs()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult supportUs(string message, HttpPostedFileBase image)
        {
            string key = "JagzH3v3wvj8nk1DS4NcwzFd3";
            string secret = "9YLUOQgwRjLoXG6GQvSTnGC9QPAF8x1KVZka9xf2h1albz7NP2";
            string token = "1106283607650381824-mcTFEj3K06QJjIjnNV9aN6rbJkF4TP";
            string tokenSecret = "p8Z7NZOttul0NuAKvRVbx2vPOdRLttwQ0pDBt2Y7FZSoR";
            string imagePath="";
            //Enter the Image Path if you want to upload image .
            if (image != null)
                 imagePath = Path.GetFullPath(image.FileName);
          
            var service = new TweetSharp.TwitterService(key, secret);
            service.AuthenticateWith(token, tokenSecret);

            //this Condition  will check weather you want to upload a image & text or only text 
            if (imagePath.Length > 0)
            {
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    var result = service.SendTweetWithMedia(new SendTweetWithMediaOptions
                    {
                        Status = message,
                        Images = new Dictionary<string, Stream> { { "john", stream } }
                    });
                }
            }
            else // just message
            {
                var result = service.SendTweet(new SendTweetOptions
                {
                    Status = message
                });

            }

           
            return View();
        }
        [Authorize]
        public ActionResult StockMarket()
        {
         
            return View();
        }
        public ActionResult NoPremission()
        {

            return View();
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}