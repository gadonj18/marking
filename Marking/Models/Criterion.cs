using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class Criterion : _BaseModel
    {
        public int ID { get; set; }
        public string FieldType { get; set; }
        public string Label { get; set; }
        public int FieldOrder { get; set; }
        public bool Default { get; set; }

        public int AssessmentID { get; set; }
        public virtual Assessment Assessment { get; set; }
        public virtual IList<Mark> Marks { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual IList<DropdownOption> Options { get; set; }
    }
}