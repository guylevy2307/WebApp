using IpWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult StockMarket()
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