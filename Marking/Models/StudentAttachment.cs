using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class StudentAttachment : _BaseAttachment
    {
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
    }
}