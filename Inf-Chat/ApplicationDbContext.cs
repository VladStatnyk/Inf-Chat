using Microsoft.AspNet.Identity.EntityFramework;
using Inf_Chat.Data.Entities;
using Inf_Chat.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Inf_Chat
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
             :base("AppConnString")
        {

        }

        public DbSet<Message> Messages { get; set; }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasRequired<User>(x => x.Author)
                .WithMany(x => x.Messages)
                .WillCascadeOnDelete(false);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}