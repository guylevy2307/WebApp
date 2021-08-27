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
using Microsoft.AspNet.Identity;

namespace IpWebApp.Controllers
{
    public class TaskController : Controller
    {
        private IpDbContext db = new IpDbContext();
        private ApplicationDbContext UserDb = new ApplicationDbContext();

        // GET: Task
        [Authorize]
        public ActionResult Index(string searchStr)
        {
            var tasks = db.Task.ToList();
            if (!string.IsNullOrEmpty(searchStr))
            {
                return View(tasks.Where(c => c.Title.ToUpper().Contains(searchStr.ToUpper())));
            }
            return View(tasks);
        }
        [Authorize]
        public ActionResult SubtaskByTaskId(int id)
        {
            Task t = db.Task.Find(id);
            if (t == null)
                return RedirectToAction("Index");
            var sub = t.Subtasks.ToList();
            return View(sub);
        }
        // GET: Task/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Include(db=>db.Record).FirstOrDefault(a=>a.TaskId==id);
             ViewBag.Users = new SelectList(UserDb.Users, "Email", "Email",User.Identity.GetUserName());
            
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Details([Bind(Include = "TaskId,Title,Description,Deadline,Pricing,Status,RecordId,Client,Assignee")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }
     */
        // GET: Task/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "Name");
            ViewBag.RecordId = new SelectList(db.Record, "RecordId", "Name");
            ViewBag.Users = new SelectList(UserDb.Users, "Email", "Email",User.Identity.GetUserName());

            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "TaskId,Title,Description,Deadline,Pricing,Status,RecordId,Client,Assignee,creatorID")] Task task)
        {

            if (ModelState.IsValid)
            {
                task.Record = db.Record.Find(task.RecordId); 
                 db.Task.Add(task);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Users = new SelectList(UserDb.Users, "Email", "Email",User.Identity.GetUserName()); 
            return View(task);
        }

        // GET: Task/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(
            [Bind(Include = "TaskId,Title,Description,Deadline,Pricing,Status,RecordId,Client,Assignee")]
            Task task)
        {
            Task temp;
            if (ModelState.IsValid)
            {
                temp = db.Task.Where(x => x.TaskId == task.TaskId).FirstOrDefault();
                if (temp != null)
                {
                    if (task.Title != temp.Title && task.Title != null)
                    {
                        temp.Title = task.Title;
                    }

                    if (task.Description != temp.Description && task.Description != null)
                    {
                        temp.Description = task.Description;
                    }

                    if (task.Deadline != temp.Deadline && task.Deadline != null)
                    {
                        temp.Deadline = task.Deadline;
                    }

                    if (task.Pricing != temp.Pricing && task.Pricing != null)
                    {
                        temp.Pricing = task.Pricing;
                    }

                    if (task.Status != temp.Status && task.Status != null)
                    {
                        temp.Status = task.Status;
                    }

                    if (task.Assignee != temp.Assignee && task.Assignee != null)
                    {
                        temp.Assignee = task.Assignee;
                    }

                }

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Users = new SelectList(UserDb.Users, "Email", "Email", User.Identity.GetUserName());
            

             temp = db.Task.Include(db => db.Record).FirstOrDefault(a => a.TaskId == task.TaskId);
            

            return View("Details", temp);
        }
    }
        // GET: Task/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            
           Task task = db.Task.Find(id);
           List<Subtask> subtasks = db.Subtasks.Where(x => x.MainTaskId == id).ToList();
            
            foreach(Subtask temp in subtasks)
            {
                db.Subtasks.Remove(temp);
            }
            db.Task.Remove(task);
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



        [HttpPost]
        [Authorize]
        public JsonResult AutoCompleteRecord(string prefix)
        {
            IpDbContext entities = new IpDbContext();
            
            var records = (from record in entities.Record
                             where record.Name.StartsWith(prefix)
                             select new
                             {
                                 label = record.Name,
                                 val = record.RecordId,
                             }).ToList();
            

            //var recordsS = db.Record.Where(x => x.Name.StartsWith(prefix)).ToList();
            //return Json(recordsS);


            return Json(records, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public JsonResult GetClientName(int id)
        {
            var codeList = db.Record.Where(i => i.RecordId == id).ToList();

            //var viewmodel = codeList.Select(x => new
            //{
            //    Id = x.ClientId,
            //    ItemName = x.Client.Name,
            //    //ItemModel = x.ItemModel,
            //    //... the other details you need

            //});
            var viewmodel = codeList.FirstOrDefault().Client.Name;

            return Json(viewmodel);
        }


        [ChildActionOnly]
        public ActionResult GetTaskTemplatesPartial()
        {

            List<TamplateTask> templateTasks = db.TamplateTask.ToList();
            return PartialView("_TemplateTasksPartial", templateTasks);
        }



        /*   public JsonResult GetTaskDeadline(int id)
           {
               var codeList = db.Record.Where(i => i.RecordId == id).ToList();

               //var viewmodel = codeList.Select(x => new
               //{
               //    Id = x.ClientId,
               //    ItemName = x.Client.Name,
               //    //ItemModel = x.ItemModel,
               //    //... the other details you need

               //});
               var viewmodel = codeList.FirstOrDefault().Client.Name;

               return Json(viewmodel);
           }*/


        [HttpPost]
        [Authorize]
        
        public JsonResult AutoCompleteTamplateTask(string prefix)
        {

            return Json(db.TamplateTask.Where(c => c.Title.StartsWith(prefix)).Select(a => new { title = a.Title, days = a.AddDays, months = a.AddMonths, years = a.AddYears, mission = a.Description, price = a.Pricing,dateType=a.dateType.ToString() }), JsonRequestBehavior.AllowGet);

        }
    }
}
