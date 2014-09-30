using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Marking.Models
{
    public class Student : _BaseModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}