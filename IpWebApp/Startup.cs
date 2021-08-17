using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using System;

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
    }
}
