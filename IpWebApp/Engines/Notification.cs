using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IpWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace IpWebApp.Engine
{
    public class Notification
    {

        private IpDbContext dbData = new IpDbContext();
        private ApplicationDbContext UserDb = new ApplicationDbContext();
        public void SendEmail()
        {
            int daysToAlertBeforeDeadline = Int32.Parse(ConfigurationManager.AppSettings["settings:daysToAlertBeforeDeadline"]);
                try{
                var userResult = UserDb.Users.ToList();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailMessage = new MailMessage();

                smtpClient.Credentials = new NetworkCredential
                {
                    UserName = "ip@cligal.com",
                    Password = "G<RdR9%G"
                };

                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                mailMessage.From = new MailAddress("ip@cligal.com");

                for (int i = 0; i < userResult.Count; i++)
                {
                    var email = userResult[i].Email;
                    mailMessage.To.Clear();
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Activity Reminder";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Hello ");

                    sb.Append(email+",\n");


                    sb.Append("Here's a list of your pending tasks" + "\n");

                    DateTime dueDate = DateTime.Now.AddDays(daysToAlertBeforeDeadline);
                    try
                    {


                        /*if (dbData.Task.Where(t => t.Assignee => z.Assignee.Equals(userResult[i]))
                            continue;*/
                        
                        if (dbData.Task.Count(z=>z.Assignee.Equals(email))==0)
                            continue;
                        List<Task> listT = dbData.Task.Where(z => z.Assignee.Equals(email)).ToList();

                        foreach (Task t in listT)
                        {
                            if (t.Deadline <= dueDate)
                            {
                                sb.Append(t.print() + "\n");
                            }
                        }

                        mailMessage.Body = sb.ToString();
                        try
                        {

                            smtpClient.Send(mailMessage);
                    
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex);
                            Console.WriteLine("cannt send email", ex);

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Empty database", ex);
                    }
                }

                }catch(Exception ex)
            {
                Console.WriteLine("Empty database", ex);
            }
        }

        public void sendPassword(String email,String pass)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailMessage = new MailMessage();

                smtpClient.Credentials = new NetworkCredential
                {
                    UserName = "ip@cligal.com",
                    Password = "G<RdR9%G"
                };

                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                mailMessage.From = new MailAddress("ip@cligal.com");
                mailMessage.To.Add(email);
                StringBuilder sb = new StringBuilder();
                sb.Append("Your link to reset password is:"+"\n"+ pass);


                mailMessage.Body = sb.ToString();
                try
                {

                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("cannt send email", ex);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Empty database", ex);
            }



        }
    }
}