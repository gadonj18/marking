using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class StudentVM
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<AttachmentVM> Attachments { get; set; }
        public ICollection<NoteVM> Notes { get; set; }
    }
}