using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class AssessmentAttachment : _BaseAttachment
    {
        public int AssessmentID { get; set; }
        public virtual Assessment Assessment { get; set; }
    }
}