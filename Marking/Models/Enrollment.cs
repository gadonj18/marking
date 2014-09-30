using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Marking.Models
{
    public class Enrollment : _BaseModel
    {
        public int ID { get; set; }
        public string Year { get; set; }

        public int ClassroomID { get; set; }
        public int StudentID { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual Student Student { get; set; }
    }
}