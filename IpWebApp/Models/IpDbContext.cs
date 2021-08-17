using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IpWebApp.Models
{
    public class IpDbContext : DbContext
    {
        public DbSet<Client> Client { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<TamplateTask> TamplateTask { get; set; }
        public DbSet<Applicant> Applicants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<IpWebApp.Models.Location> Locations { get; set; }
    }
}