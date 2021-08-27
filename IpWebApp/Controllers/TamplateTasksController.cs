using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IpWebApp.Models;

namespace IpWebApp.Controllers
{
    public class TamplateTasksController : Controller
    {
        private IpDbContext db = new IpDbContext();

        /*
         * ▶▶▶▶▶▶▶ PAY ATTENTION!
         * TEMPLATE TASKS INDEX VIEW IS CANCELLED!
         * DO NO NOT USE IT ANYMORE,
         * FROM NOW ON TEMPLATE TASKS INDEX VIEW IS 
         * Task/_TemplateTasksPartial.cshtml
         * IM NOT DELETING TamplateTasks/Index.cshtml
         * TO MAKE SURE NOTHING BREAKS. BUT DONT USE THAT FILE ANYMORE ◀◀◀◀◀◀◀
         * THANKS
         * ALON
         */

        // GET: TamplateTasks
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Task");
        }

        // GET: TamplateTasks/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TamplateTask tamplateTask = db.TamplateTask.Find(id);
            if (tamplateTask == null)
            {
                return HttpNotFound();
            }
            return View(tamplateTask);
        }

        // GET: TamplateTasks/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TamplateTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TamplateTaskId,Title,Description,AddDays,AddMonths,AddYears,Pricing,dateType,creatorId")] TamplateTask tamplateTask)
        {
            if (ModelState.IsValid)
            {
                db.TamplateTask.Add(tamplateTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tamplateTask);
        }

        // GET: TamplateTasks/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TamplateTask tamplateTask = db.TamplateTask.Find(id);
            if (tamplateTask == null)
            {
                return HttpNotFound();
            }
            return View(tamplateTask);
        }

        // POST: TamplateTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TamplateTaskId,Title,Description,AddDays,AddMonths,AddYears,Pricing,dateType")] TamplateTask tamplateTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tamplateTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          else  return View("Details",tamplateTask);
        }

        // GET: TamplateTasks/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TamplateTask tamplateTask = db.TamplateTask.Find(id);
            if (tamplateTask == null)
            {
                return HttpNotFound();
            }
            return View(tamplateTask);
        }

        // POST: TamplateTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TamplateTask tamplateTask = db.TamplateTask.Find(id);
            db.TamplateTask.Remove(tamplateTask);
            db.SaveChanges();
            return RedirectToAction("Index");
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
