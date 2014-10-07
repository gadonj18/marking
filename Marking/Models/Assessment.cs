using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class Assessment : _BaseModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public bool GroupWork { get; set; }
        public DateTime? DateDue { get; set; }

        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual IList<Criterion> Criteria { get; set; }
    }
}