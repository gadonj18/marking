using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public enum AttachmentTypes { PDF, DOC, PPT, XLS, OtherDocument, Image }

    public class Attachment : _BaseModel
    {
        public int ID { get; set; }
        public AttachmentTypes Type { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }
        public int Filesize { get; set; }
    }
}