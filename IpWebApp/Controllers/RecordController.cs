using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IpWebApp.Models;
using System.IO;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IpWebApp.Controllers
{
    public class RecordController : Controller
    {
        private IpDbContext db = new IpDbContext();

        // GET: Record
        [Authorize]

        public ActionResult Index(string searchStr, string key, DateTime? start, DateTime? end, string onDate, string onString, string dateKey)
        {
            var records = db.Record.ToList();
            if (onString != null)
            {
                if (!string.IsNullOrEmpty(searchStr))
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (key.Equals("name"))
                            return View(records.Where(c => c.Name.ToUpper().Contains(searchStr.ToUpper())));
                        else
                        {
                            if (key.Equals("number"))
                            {

                                return View(records.Where(c => c.RegistrationNumber != null && c.RegistrationNumber.Contains(searchStr)));

                            }
                        }
                    }
                }
            }
            else if (onDate != null)
            {
                if (start != null && end != null)
                {
                    if (dateKey.Equals("Application"))
                    {
                        return View(records.Where(c => c.ApplicationDate > start).Where(c => c.ExpirationDate < end));

                    }
                    else if (dateKey.Equals("Registration"))
                    {
                        return View(records.Where(c => c.RegistrationDate > start).Where(c => c.RegistrationDate < end));

                    }
                    else if (dateKey.Equals("Renewal"))
                    {
                        return View(records.Where(c => c.RenewalDate > start).Where(c => c.RenewalDate < end));

                    }
                    else if (dateKey.Equals("NextAction"))
                    {
                        return View(records.Where(c => c.NextActionDate > start).Where(c => c.NextActionDate < end));

                    }
                    else if (dateKey.Equals("Expiration"))
                    {
                        return View(records.Where(c => c.ExpirationDate > start).Where(c => c.ExpirationDate < end));

                    }

                }
            }

            return View(records);
        }

        /* public ActionResult Index(DateTime? start, DateTime? end)
         {
             var records = db.Record.ToList();
             if (start != null&& end != null)
             {
                 return View(records.Where(c => c.ApplicationDate> start).Where(c=>c.ExpirationDate< end));
             }
             return View(records);
         }*/

        // GET: Record/Details/5
        [Authorize]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "Name", record.ClientId);

            return View(record);
        }

        // GET: Record/Create
        [Authorize]

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "Name");
            return View();
        }

        [Authorize]
        public ActionResult CreateById(int? Id)
        {
            ViewBag.RecordId = new SelectList(db.Record.Where(r => r.RecordId == Id), "RecordId", "Name");
            Record divisionalRecord = db.Record.FirstOrDefault(r => r.RecordId == Id);
            //checking if the record is existing
            if (divisionalRecord != null)
            {
                ViewData["recordName"] = divisionalRecord.Name;
                return View(divisionalRecord);

            }
            return View();
        }

        // POST: Record/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult Create([Bind(Include = "RecordId,Type,Name,RegistrationDate,RenewalDate,RegistrationNumber,ClientId,Inventor,Classes,ApplicationDate,ExpirationDate,NextActionDate,Country,Notes,Priority,PriorityCountry,PriorityNumber,PriorityDate,ParentId,creatorId")] Record record, [Bind(Include = /*ApplicantId,*/"ApplicantName,Phone,Address,Formation,POA")] Applicant app, [Bind(Include = "ApplicantIdList")] string ApplicantIdList)
        {
            record.Client = db.Client.Find(record.ClientId);
            record.Parent = db.Record.Find(record.ParentId);


            if (ModelState.IsValid)
            {
                //add Image 
                HttpPostedFileBase file = Request.Files["image"];
                if (file != null)
                {
                    // image sent
                    if (file.ContentLength > 0)
                    {
                        // add image to record
                        record.Image = new byte[file.ContentLength];
                        file.InputStream.Read(record.Image, 0, file.ContentLength);
                    }
                    // no image sent
                    else if (file.ContentLength < 1)
                    {
                        // check if record have a parent
                        if (record.ParentId != null)
                        {
                            // sets the parent's image to record
                            var image = GetImageFromDataBase(record.ParentId.GetValueOrDefault()); // .GetValueOrDefault() coverts int? into regular int
                            record.Image = image;
                        }
                        // if no image sent and there is no parent's image sets the record's image to 
                        // an empty image (an empty array)
                        else if (record.ParentId == null)
                        {
                            record.Image = new byte[file.ContentLength];
                            file.InputStream.Read(record.Image, 0, file.ContentLength);
                        }

                    }
                }

                //add aplicant
                AddApplicantLocal(record, app);
                return RedirectToAction("Details", new { id = record.RecordId });
            }
            return View(record);

            //converting string to int list
            #region AddApplicant local method
            void AddApplicantLocal(Record AddAppRecord, Applicant AddApp)
            {
                // retrieve applicant fron db 
                //var checkApplicant = db.Applicants.Find(AddApp.ApplicantName/*, AddApp.Phone,AddApp.Address ,AddApp.Formation,AddApp.POA*/); 
                var checkApplicant = db.Applicants.FirstOrDefault(name => name.ApplicantName == AddApp.ApplicantName);

                #region Applicant not found
                //check if applicant exists
                if (checkApplicant == null)
                {
                    // if it doesnt check if new applicant details were recieved
                    if (AddApp.ApplicantName != "" && !AddApp.ApplicantName.IsNullOrWhiteSpace())
                    {
                        // if so, add new applicant to db
                        AddApp.ApplicantId = db.Applicants.Count() + 1;
                        db.Applicants.Add(AddApp);
                        db.SaveChanges();
                    }

                }
                #endregion
                // get applicant from db after it was found or created
                Applicant sentApplicant = db.Applicants.Find(AddApp.ApplicantId);

                if (AddAppRecord.Applicants == null)
                {
                    AddAppRecord.Applicants = new List<Applicant>();
                }
                #region Applicant found
                #region Create list of Ids
                var _applicantIntIds = StringToIntList(ApplicantIdList);
                //check if sent applicant id already exists in the list
                int _sentApplicantId = default; 
                if (sentApplicant != null)
                {
                     _sentApplicantId = sentApplicant.ApplicantId;
                }
                if (!_applicantIntIds.Contains(_sentApplicantId))
                { // if not add it
                    _applicantIntIds.Add(_sentApplicantId);
                }
                // removes duplicate entries 
                var distinctIds = _applicantIntIds.Distinct().ToList();
                #endregion

                foreach (var appId in distinctIds)
                {
                    Applicant applicant = db.Applicants.Find(appId);
                    // var applicantType = applicant.GetType();
                    if (applicant != null/* && (applicantType == typeof(Applicant))*/)
                    {
                        AddAppRecord.Applicants.Add(applicant);
                        applicant.Records.Add(AddAppRecord);
                    }
                }
                #endregion

                db.Record.Add(AddAppRecord);
                db.SaveChanges();

            }
            #endregion
            List<int> StringToIntList(string strToInt)
            {
                var _strToInt = strToInt.Split(',');
                List<int> _intFromStrList = new List<int>();
                foreach (var str in _strToInt)
                {
                    if (int.TryParse(str, out int strInt)) _intFromStrList.Add(strInt);
                }
                return _intFromStrList;
            }
        }

        [HttpPost]
        public JsonResult PostApplicantsJson(string appJson)
        {
            //! two way of parssing this json. one is with a custom Newtonsoft JsonConvert
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var parsedApplicant = JsonConvert.DeserializeObject<Applicant>(appJson, settings);

            //! the other is with a new Applicant built through a JObject
            #region JObject option
            // dynamic jApplicant = JObject.Parse(appJson);
            /* Applicant parsedApplicant1 = new Applicant()
            {
               ApplicantName = jApplicant.ApplicantName,
               ApplicantId = jApplicant.ApplicantId,
               Phone = jApplicant.Phone,
               Address = jApplicant.Address,
               Formation = jApplicant.Formation,
               POA = jApplicant.POA
            };
            */
            #endregion

            // retrieve applicant fron db 
            var checkApplicant = db.Applicants.Find(parsedApplicant.ApplicantId);
            var checkApplicant2 = db.Applicants.FirstOrDefault(name => name.ApplicantName == parsedApplicant.ApplicantName);

            //check if applicant exists
            if (checkApplicant == null && checkApplicant2 == null)
            {
                // if it doesnt check if new applicant details were recieved
                if (parsedApplicant.ApplicantName != "" && !parsedApplicant.ApplicantName.IsNullOrWhiteSpace())
                {
                    // if so, add new applicant to db
                    parsedApplicant.ApplicantId = db.Applicants.Count() + 1;
                    db.Applicants.Add(parsedApplicant);
                    db.SaveChanges();
                    return Json(parsedApplicant);
                }
            }
            return Json(parsedApplicant);
        }

        [Authorize]
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            var dbImgMineType = FileValidation.getMimeFromFile(cover);
            Console.WriteLine("Image type is: ", dbImgMineType);
            if (cover != null)
            {
                return File(cover, dbImgMineType);
            }
            else
            {
                return null;
            }
        }

        [Authorize]
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Record where temp.RecordId == Id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        //this function add applicant to the list of records and add record to the list of applicant
        [Authorize]
        [HttpPost]
        public ActionResult AddApplicant(String ApplicantId, String RecordId)
        {

            Applicant applicant = db.Applicants.Find(Int32.Parse(ApplicantId));
            Record rec = db.Record.Find(Int32.Parse(RecordId));
            if (applicant.Records == null)
            {
                applicant.Records = new List<Record>();
            }
            if (rec.Applicants == null)
            {
                rec.Applicants = new List<Applicant>();
            }
            applicant.Records.Add(rec);
            rec.Applicants.Add(applicant);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = Int32.Parse(RecordId) });

        }



        // GET: Record/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Include(db => db.Applicants).FirstOrDefault(x => x.RecordId == id);
            if (record == null)
            {
                return HttpNotFound();
            }
            //checking if the user has permissions
            if (!(record.creatorId.Equals(User.Identity.Name))||!User.IsInRole("Admin") )
            {
                return RedirectToAction("NoPremission","Home");
            }
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "Name", record.ClientId);
            ViewBag.ParentId = new SelectList(db.Record, "ParentId", "Name", record.ParentId);
            return View(record);
        }

        // POST: Record/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,Name,RegistrationDate,RenewalDate,RegistrationNumber,ClientId,ApplicationDate,NextActionDate,Country,Notes,Priority,PriorityCountry,PriorityNumber,PriorityDate,ParentId")] Record record)
        {
            if (ModelState.IsValid)
            {
                var temp = db.Record.Where(x => x.RecordId == record.RecordId).FirstOrDefault();
                if (temp != null)
                {
                    if (record.Name != temp.Name && record.Name != null)
                    {
                        temp.Name = record.Name;
                    }

                    if (record.ClientId != temp.ClientId && record.Name != null)
                    {
                        temp.ClientId = record.ClientId;
                        temp.Client = db.Client.Find(record.ClientId);
                    }
                    if (record.NextActionDate != temp.NextActionDate && record.NextActionDate != null)
                    {
                        temp.NextActionDate = record.NextActionDate;
                    }
                    if (record.RegistrationNumber != temp.RegistrationNumber && record.RegistrationNumber != null)
                    {
                        temp.RegistrationNumber = record.RegistrationNumber;
                    }
                    if (record.ApplicationDate != temp.ApplicationDate && record.ApplicationDate != null)
                    {
                        temp.ApplicationDate = record.ApplicationDate;
                    }
                    if (record.Country != temp.Country && record.Country != null)
                    {
                        temp.Country = record.Country;
                    }
                    if (record.ExpirationDate != temp.ExpirationDate && record.ExpirationDate != null)
                    {
                        temp.ExpirationDate = record.ExpirationDate;
                    }
                    if (record.RegistrationDate != temp.RegistrationDate && record.RegistrationDate != null)
                    {
                        temp.RegistrationDate = record.RegistrationDate;
                    }
                    if (record.RenewalDate != temp.RenewalDate && record.RenewalDate != null)
                    {
                        temp.RenewalDate = record.RenewalDate;
                    }
                    if (record.Notes != temp.Notes && record.Notes != null)
                    {
                        temp.Notes = record.Notes;
                    }
                    if (record.Priority != temp.Priority && record.Priority != null)
                    {
                        temp.Priority = record.Priority;
                    }
                    if (record.PriorityCountry != temp.PriorityCountry && record.PriorityCountry != null)
                    {
                        temp.PriorityCountry = record.PriorityCountry;
                    }
                    if (record.PriorityNumber != temp.PriorityNumber && record.PriorityNumber != null)
                    {
                        temp.PriorityNumber = record.PriorityNumber;
                    }
                    if (record.PriorityDate != temp.PriorityDate && record.PriorityDate != null)
                    {
                        temp.PriorityDate = record.PriorityDate;
                    }

                    if (record.ParentId != temp.ParentId && record.ParentId != null)
                    {
                        temp.ParentId = record.ParentId;
                        temp.Parent = db.Record.Find(record.ParentId);
                    }
                    HttpPostedFileBase file = Request.Files["image"];

                    if (file != null && file.ContentLength > 0)
                    {
                        temp.Image = new byte[file.ContentLength];
                        file.InputStream.Read(temp.Image, 0, file.ContentLength);
                    }
                }
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "RecordId,Name,RegistrationDate,RenewalDate,ExpirationDate,RegistrationNumber,ClientId,ApplicationDate,NextActionDate,Country,Notes,Priority,PriorityCountry,PriorityNumber,PriorityDate,ParentId")] Record record)
        {
            if (ModelState.IsValid)
            {
                var temp = db.Record.Where(x => x.RecordId == record.RecordId).FirstOrDefault();
                if (temp != null)
                {
                    if (record.Name != temp.Name && record.Name != null)
                    {
                        temp.Name = record.Name;
                    }

                    if (record.ClientId != temp.ClientId && record.Name != null)
                    {
                        temp.ClientId = record.ClientId;
                        temp.Client = db.Client.Find(record.ClientId);
                    }
                    if (record.NextActionDate != temp.NextActionDate && record.NextActionDate != null)
                    {
                        temp.NextActionDate = record.NextActionDate;
                    }
                    if (record.RegistrationNumber != temp.RegistrationNumber && record.RegistrationNumber != null)
                    {
                        temp.RegistrationNumber = record.RegistrationNumber;
                    }
                    if (record.Country != temp.Country && record.Country != null)
                    {
                        temp.Country = record.Country;
                    }
                    if (record.ExpirationDate != temp.ExpirationDate && record.ExpirationDate != null)
                    {
                        temp.ExpirationDate = record.ExpirationDate;
                    }
                    if (record.RegistrationDate != temp.RegistrationDate && record.RegistrationDate != null)
                    {
                        temp.RegistrationDate = record.RegistrationDate;
                    }
                    if (record.RenewalDate != temp.RenewalDate && record.RenewalDate != null)
                    {
                        temp.RenewalDate = record.RenewalDate;
                    }
                    if (record.ExpirationDate != temp.ExpirationDate && record.ExpirationDate != null)
                    {
                        temp.ExpirationDate = record.ExpirationDate;
                    }
                    if (record.ApplicationDate != temp.ApplicationDate && record.ApplicationDate != null)
                    {
                        temp.ApplicationDate = record.ApplicationDate;
                    }
                    if (record.Notes != temp.Notes && record.Notes != null)
                    {
                        temp.Notes = record.Notes;
                    }
                    if (record.Priority != temp.Priority && record.Priority != null)
                    {
                        temp.Priority = record.Priority;
                    }
                    if (record.PriorityCountry != temp.PriorityCountry && record.PriorityCountry != null)
                    {
                        temp.PriorityCountry = record.PriorityCountry;
                    }
                    if (record.PriorityNumber != temp.PriorityNumber && record.PriorityNumber != null)
                    {
                        temp.PriorityNumber = record.PriorityNumber;
                    }
                    if (record.PriorityDate != temp.PriorityDate && record.PriorityDate != null)
                    {
                        temp.PriorityDate = record.PriorityDate;
                    }
                    if (record.ParentId != temp.ParentId && record.ParentId != null)
                    {
                        temp.ParentId = record.ParentId;
                        temp.Parent = db.Record.Find(record.ParentId);
                    }
                    HttpPostedFileBase file = Request.Files["image"];
                    if (file != null && file.ContentLength > 0)
                    {
                        temp.Image = new byte[file.ContentLength];
                        file.InputStream.Read(temp.Image, 0, file.ContentLength);
                    }
                }
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(record);
        }

        // GET: Record/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record r = db.Record.Find(id);
            List<Task> tasks = db.Task.Where(y => y.RecordId == id).ToList();
            foreach (Task ta in tasks)
            {
                List<Subtask> subtasks = db.Subtasks.Where(x => x.MainTaskId == ta.TaskId).ToList();
                foreach (Subtask temp in subtasks)
                {
                    db.Subtasks.Remove(temp);
                }
                db.Task.Remove(ta);
            }
            List<Record> Children = db.Record.Where(child => child.ParentId == id).ToList();
            foreach (var child in Children)
            {
                db.Record.Find(child.RecordId).ParentId = null;
            }
            db.Record.Remove(r);
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
        public JsonResult GetCountriesByPrefix(string prefix)
        {
            var regulation = GetRegulation();
            try
            {

                if (String.IsNullOrEmpty(prefix))
                {
                    return Json(regulation, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var filteredList = regulation.Where(c => c.CountryName.StartsWith(char.ToUpper(prefix[0]) + prefix.Substring(1))).ToList();
                    return Json(filteredList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return Json(regulation, JsonRequestBehavior.AllowGet);
            }
            //  var csvData = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/country.csv"));




        }
        [HttpPost]
        public JsonResult GetClientByPrefix(string prefix)
        {
            return Json(db.Client.Where(c => c.Name.StartsWith(prefix)).Select(a => new { label = a.Name, value = a.ClientId }), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetRecordByPrefix(string prefix)
        {
            var temp = db.Record.Where(c => c.Name.StartsWith(prefix));
            return Json(temp.Select(a => new
            {
                label = a.Name,
                value = a.RecordId,
                nextAction = a.NextActionDate,
                RenewalDate = a.RenewalDate,
                RegistrationDate = a.RegistrationDate,
                ParentId = a.ParentId,
                Notes = a.Notes,
                RegistrationNumber = a.RegistrationNumber.ToString(),
                ClientId = a.ClientId,
                ClientName = a.Client.Name,
                Territory = a.Country,
                Priority = a.Priority
            }), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetRecordByPrefixJson(string prefix)
        {
            var temp = db.Record.Where(c => c.Name.StartsWith(prefix));
            return Json(temp.Select(a => new
            {
                value = a.Name,
                RecordId = a.RecordId,
                nextAction = a.NextActionDate,
                RenewalDate = a.RenewalDate,
                RegistrationDate = a.RegistrationDate,
                ParentId = a.ParentId,
                Notes = a.Notes,
                RegistrationNumber = a.RegistrationNumber.ToString(),
                ClientId = a.ClientId,
                ClientName = a.Client.Name,
                Territory = a.Country,
                Priority = a.Priority,
                PriorityCountry = a.PriorityCountry,
                PriorityNumber = a.PriorityNumber,
                PriorityDate = a.PriorityDate,
                Image = a.Image,
                Inventor = a.Inventor
            }), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetApplicantsByPrefix(string prefix)
        {
            var temp = db.Applicants.Where(c => c.ApplicantName.StartsWith(prefix));
            return Json(temp.Select(a => new { label = a.ApplicantName, value = a.ApplicantId }), JsonRequestBehavior.AllowGet);

        }

        private List<Country> GetRegulation()
        {
            List<Country> regulation = new List<Country>();
            using (var csvData = new StreamReader(Server.MapPath(@"~/App_Data/Regulation.csv")))
            {
                String line = csvData.ReadLine();
                // Read the stream as a string, and write the string to the console.
                while (line != null && line.Length > 1)
                {
                    String[] cell = line.Split(',');
                    if (!string.IsNullOrEmpty(cell[0]) && !string.IsNullOrEmpty(cell[1]) && !string.IsNullOrEmpty(cell[2]))
                    {
                        regulation.Add(new Country { Id = Int32.Parse(cell[0]), CountryName = cell[1], RenewalPeriod = Int32.Parse(cell[2]) });
                        line = csvData.ReadLine();
                    }

                }
            }
            return regulation;
        }
    }

    internal class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public int RenewalPeriod { get; set; }
    }
}
