using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IpWebApp.Models;
using Microsoft.Ajax.Utilities;

namespace IpWebApp.Controllers
{
    public class ClientController : Controller
    {
        private IpDbContext db = new IpDbContext();

        // GET: Client
        [Authorize]
        public ActionResult Index(string searchStr, string onBalance, string balanceKey, string balance)
        {
            var clients = db.Client.ToList();
            if (onBalance != null)
            {
                double b=double.Parse(balance);
                if (b!=null)
                {
                    switch (balanceKey)
                    {
                        case "more then":
                            return View(clients.Where(c => c.Balance > b));
                            break;
                        case "less then":
                            return View(clients.Where(c => c.Balance < b));
                            break;
                        case "exactly":
                            return View(clients.Where(c => c.Balance == b));
                            break;

                    }
                 
                }
            }

            else if (!string.IsNullOrEmpty(searchStr))
            {
                return View(clients.Where(c => c.Name.ToUpper().Contains(searchStr.ToUpper())));
            }
            return View(clients);
        }

        //this function return a list of record by client id
        [Authorize]
        public ActionResult RecordsByClientId(int id)
        {
            Client c = db.Client.Find(id);
            var records = c.Records.ToList();
            return View(records); 
        }

      
        [ChildActionOnly]
        public ActionResult TasksByClientIdPartial(int id)
        {

            List<Task> tasks = db.Task.Where(x => x.Record.ClientId == id).ToList();
            return PartialView("_TasksByClientIdPartial", tasks);
        }

        [ChildActionOnly]
        public ActionResult ApplicantsByClientIdPartial(int id)
        {
            Client client  = db.Client.Find(id);
           var clientApplicantsByItsRecords = client.Records.SelectMany(record => record.Applicants).ToList();
            return PartialView("_ApplicantsByClientIdPartial", clientApplicantsByItsRecords);
        }



        // GET: Client/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Client/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ClientId,Name,ContactName,ContactEmail,ContactPosition,BillingName,BillingEmail,Currency,Referent,Balance,Notes,creatorId")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Client/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            if (!(client.creatorId.Equals(User.Identity.Name) || User.IsInRole("Admin")))
            {
                return RedirectToAction("NoPremission", "Home");
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ClientId,Name,ContactName,ContactEmail,ContactPosition,BillingName,BillingEmail,Currency,Referent,Balance,vatNumber,Notes")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }


        // GET: Client/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Client.Find(id);
            if (client != null)
            {

                List<Record> records = db.Record.Where(x => x.ClientId == id).ToList();

                foreach (Record r in records)
                {
                    List<Task> tasks = db.Task.Where(y => y.RecordId == r.RecordId).ToList();
                    foreach (Task ta in tasks)
                    {
                        List<Subtask> subtasks = db.Subtasks.Where(x => x.MainTaskId == ta.TaskId).ToList();
                        foreach (Subtask temp in subtasks)
                        {
                            db.Subtasks.Remove(temp);
                        }
                        db.Task.Remove(ta);
                    }
                    db.Record.Remove(r);
                }

                db.Locations.Remove(client.Location);
                db.Client.Remove(client);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [ChildActionOnly]
        public ActionResult LocationByClientIdPartial(int id)
        {

            Location gps = db.Locations.FirstOrDefault(x => x.Client.ClientId == id);
            if(gps==null)
                return PartialView("LocationByClientIdPartial", null);
            else 
                return PartialView("LocationByClientIdPartial", gps);
        }
        [Authorize]
        public ActionResult GenerateReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);            //Client client = new Client() { Name = "Teva", ClientId = 3, Email = "legal@teva.com"};
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
