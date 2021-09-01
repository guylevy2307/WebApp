using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using System;
using IpWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartupAttribute(typeof(IpWebApp.Startup))]
namespace IpWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration
            .UseSqlServerStorage(
                "IpDbContext",
                new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });
            createRolesandUsers();

            //app.UseHangfireDashboard();
            //using (var connection = JobStorage.Current.GetConnection())
            //{
            //    foreach (var recurringJob in connection.GetRecurringJobs())
            //    {
            //        RecurringJob.RemoveIfExists(recurringJob.Id);
            //    }
            //}
            ////BackgroundJob.Enqueue(() => Console.WriteLine("fire-and-forget!") );
            //app.UseHangfireServer();

            //Engine.Notification temp = new Engine.Notification();
            ////  RecurringJob.AddOrUpdate(() => temp.SendEmail(),Cron.Minutely());
            //RecurringJob.AddOrUpdate(() => temp.SendEmail(), Cron.Daily(9));

           
        }
        // In this method we will create default User roles and Admin user for login    
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            roleManager.Create(new IdentityRole { Name = "Admin" });
            roleManager.Create(new IdentityRole { Name = "Client" });
         

          
        }
        }
}
