using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IpWebApp.Models;

namespace IpWebApp.Controllers
{
    public class SubtaskController : Controller
    {
        private IpDbContext db = new IpDbContext();

        // GET: Subtasks
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Subtasks.ToList());
        }

        // GET: Subtasks/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subtask subtask = db.Subtasks.FirstOrDefault(x=>x.SubtaskId==id);
            if (subtask == null)
            {
                return HttpNotFound();
            }
            return View(subtask);
        }

        // GET: Subtasks/Create
        [Authorize]
        public ActionResult Create()

        {
            return RedirectToAction("Index");
            //ViewBag.TaskId = new SelectList(db.Task, "TaskId", "Title").ToList();
           
        }
        [Authorize]
        public ActionResult CreateById(int? Id)
        {
            ViewBag.TaskId = new SelectList(db.Task.Where(x => x.TaskId == Id), "TaskId", "Title");
            Task t = db.Task.FirstOrDefault(x => x.TaskId == Id);
            if(t!=null) ViewData["taskName"] =t.Record.Name;
            return View();
        }

       

        // POST: Subtasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "SubtaskId,Title,Description,Deadline,Status,MainTaskId,Pricing,creatorID")] Subtask subtask)
        {
            subtask.MainTask = db.Task.FirstOrDefault(x=>x.TaskId==subtask.MainTaskId);
            subtask.Status = TaskStatus.Pending;
            if (ModelState.IsValid)
            {
                db.Subtasks.AddOrUpdate(subtask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaskId = new SelectList(db.Task.Where(x => x.TaskId == subtask.MainTaskId), "TaskId", "Title");
            return View("CreateById", subtask);
        }

        // GET: Subtasks/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subtask subtask = db.Subtasks.FirstOrDefault(x=>x.SubtaskId==id);
            if (subtask == null)
            {
                return HttpNotFound();
            }
            //checking if there is permission to the user for edit
            if (!(subtask.creatorId.Equals(User.Identity.Name)) || User.IsInRole("Admin"))
            {
                return RedirectToAction("NoPremission", "Home");
            }
            return View(subtask);
        }

        // POST: Subtasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "SubtaskId,Title,Description,Deadline,Status,MainTaskId")] Subtask subtask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subtask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subtask);
        }

        // GET: Subtasks/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subtask subtask = db.Subtasks.Find(id);
            if (subtask == null)
            {
                return HttpNotFound();
            }
            return View(subtask);
        }

        // POST: Subtasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Subtask subtask = db.Subtasks.Find(id);
            db.Subtasks.Remove(subtask);
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
