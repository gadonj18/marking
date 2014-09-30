using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Marking.Models
{
    public class Mark : _BaseModel
    {
        public int ID { get; set; }
        public string Value { get; set; }

        public int CriterionID { get; set; }
        public virtual Criterion Criterion { get; set; }
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}