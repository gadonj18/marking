using Marking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class AttachmentVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }
        public bool Removed { get; set; }
        public HttpPostedFileBase File { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}