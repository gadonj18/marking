using Marking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Web;

namespace Marking.DAL
{
    public class MarkingContext : DbContext
    {
        public MarkingContext() : base("MarkingContext")  { }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssessmentAttachment> AssessmentAttachments { get; set; }
        public DbSet<StudentAttachment> StudentAttachments { get; set; }
        public DbSet<ClassroomAttachment> ClassroomAttachments { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Criterion> Criteria { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<DropdownOption> DropdownOptions { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Student> Students { get; set; }

        public string UploadAttachment(HttpPostedFileBase File)
        {
            if (File.ContentLength <= 0)
            {
                throw new Exception("Empty file");
            }
            HttpServerUtility Server = HttpContext.Current.Server;
            string Filename = Path.GetFileName(File.FileName);
            string FilenameInternal = Filename;
            while(System.IO.File.Exists(Server.MapPath("~/Content/uploads/" + FilenameInternal)))
            {
                FilenameInternal = Guid.NewGuid().ToString() + "_" + Filename;
            }
            string path = Path.Combine(Server.MapPath("~/Content/uploads"), FilenameInternal);
            File.SaveAs(path);
            return FilenameInternal;
        }

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