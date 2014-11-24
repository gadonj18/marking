using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Marking.Models
{
    public class Classroom : _BaseModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Grade { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Assessment> Assessments { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<ClassroomAttachment> Attachments { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}