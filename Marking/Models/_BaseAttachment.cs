using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class _BaseAttachment : _BaseModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }
        public string FilenameInternal { get; set; }
    }
}