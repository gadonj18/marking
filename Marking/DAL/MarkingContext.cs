using Marking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Marking.DAL
{
    public class MarkingContext : DbContext
    {
        public MarkingContext() : base("MarkingContext")  { }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Criterion> Criteria { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<DropdownOption> DropdownOptions { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            DateTime now = DateTime.Now;
            foreach (var entity in ChangeTracker.Entries<_BaseModel>().Where(e => e.State == EntityState.Modified).Select(e => e.Entity))
            {
                entity.DateModified = now;
            }
            foreach (var entity in ChangeTracker.Entries<_BaseModel>().Where(e => e.State == EntityState.Added).Select(e => e.Entity))
            {
                entity.DateCreated = now;
            }

            return base.SaveChanges();
        }
    }
}