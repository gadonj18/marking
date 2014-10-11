using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class Attachment : _BaseModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string ParentModel { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }
        public string FilenameInternal { get; set; }
    }
}