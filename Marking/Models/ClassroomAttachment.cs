using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class ClassroomAttachment : _BaseAttachment
    {
        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }
    }
}