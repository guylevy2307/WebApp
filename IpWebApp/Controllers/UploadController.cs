
using IpWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IpWebApp.Controllers
{
    public class UploadController : Controller
    {
        private IpDbContext db = new IpDbContext();

        // GET: Home
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {

                using (var streamReader = new StreamReader(postedFile.InputStream))
                {
                    string line;
                    string[] lineFeaturs = streamReader.ReadLine().Split(',');

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] value = line.Split(',');
                        if (value[0].Equals("record"))
                        {
                            Record temp = new Record();
                            for (int i = 1; i < lineFeaturs.Length; i++)
                            {
                                if (!value[i].Equals(""))
                                {


                                    //add info to temp
                                    if (string.Equals(lineFeaturs[i], nameof(Record.RecordId), StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (db.Record.Find(int.Parse(value[i])) != null)
                                        {
                                            ViewBag.MessegeExisct = "You have Recored with the same Id";
                                        }
                                        else temp.RecordId = Int32.Parse(value[i]);
                                    }
                                    else
                                 if (string.Equals(lineFeaturs[i], nameof(Record.ClientId), StringComparison.OrdinalIgnoreCase))

                                    {
                                        Client c = db.Client.Find(Int32.Parse(value[i]));
                                        if (c != null)
                                        {
                                            temp.ClientId = Int32.Parse(value[i]);
                                            temp.Client = c;
                                        }
                                        else
                                        {
                                            ViewBag.MessageError = "Must have ClientId or Worrng ClientId ";
                                            return View();
                                        }
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.Name), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.Name = (value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.ApplicationDate), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.ApplicationDate = DateTime.Parse(value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.RegistrationNumber), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.RegistrationNumber = (value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.RenewalDate), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.RenewalDate = DateTime.Parse(value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.NextActionDate), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.NextActionDate = DateTime.Parse(value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], "Record Type", StringComparison.OrdinalIgnoreCase) ||
                                          string.Equals(lineFeaturs[i], nameof(RecordType), StringComparison.OrdinalIgnoreCase))

                                    {
                                        if (RecordType.Design.Equals(value[i]))
                                        {
                                            temp.Type = RecordType.Design;
                                        }
                                        else if (RecordType.Patent.Equals(value[i]))
                                        {
                                            temp.Type = RecordType.Patent;
                                        }
                                        else if (RecordType.Trademark.Equals(value[i]))
                                        {
                                            temp.Type = RecordType.Trademark;
                                        }
                                    }

                                    else if (string.Equals(lineFeaturs[i], nameof(Country), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.Country = (value[i]);
                                    }

                                    else if (string.Equals(lineFeaturs[i], nameof(Record.Inventor), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.Inventor = (value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.Classes), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.Classes = (value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.ExpirationDate), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.ExpirationDate = DateTime.Parse(value[i]);
                                    }
                                    else if (string.Equals(lineFeaturs[i], nameof(Record.Notes), StringComparison.OrdinalIgnoreCase))
                                    {
                                        temp.Notes = (value[i]);
                                    }
                                }
                            }
                            if (!temp.RecordId.Equals(""))
                                db.Record.Add(temp);

                        }

                        else
                        {
                            Client temp = new Client();
                            for (int i = 1; i < lineFeaturs.Length; i++)
                            {
                                //add info to temp
                                if (!value[i].Equals(""))
                                {


                                    if (lineFeaturs[i] == nameof(Client.ClientId))
                                    {
                                        temp.ClientId = Int32.Parse(value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.Name)))
                                    {
                                        temp.Name = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.ContactName)))
                                    {
                                        temp.ContactName = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.ContactEmail)))
                                    {
                                        temp.ContactEmail = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.ContactPosition)))
                                    {
                                        temp.ContactPosition = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.BillingName)))
                                    {
                                        temp.BillingName = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.BillingEmail)))
                                    {
                                        temp.BillingEmail = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.VatNumber)))
                                    {
                                        temp.VatNumber = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.Referent)))
                                    {
                                        temp.Referent = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.Notes)))
                                    {
                                        temp.Notes = (value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Currency)))
                                    {
                                        if (Currency.ILS.Equals(value[i]))
                                        {
                                            temp.Currency = Currency.ILS;
                                        }
                                        else if (Currency.USD.Equals(value[i]))
                                        {
                                            temp.Currency = Currency.USD;
                                        }
                                        else if (Currency.EUR.Equals(value[i]))
                                        {
                                            temp.Currency = Currency.EUR;
                                        }
                                    }

                                    else if (lineFeaturs[i].Equals(nameof(Client.Balance)))
                                    {
                                        temp.Balance = Double.Parse(value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.PaymentIssued)))
                                    {
                                        temp.PaymentIssued = Double.Parse(value[i]);
                                    }
                                    else if (lineFeaturs[i].Equals(nameof(Client.PaymentReceived)))
                                    {
                                        temp.PaymentReceived = Double.Parse(value[i]);
                                    }
                                }
                            }
                            if (!temp.ClientId.Equals(""))
                                db.Client.Add(temp);


                        }
                    }
                }


                db.SaveChanges();
                ViewBag.Message = "File uploaded successfully.";
            }

            return View();
        }

    }
}