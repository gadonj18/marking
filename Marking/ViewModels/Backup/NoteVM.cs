using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class NoteVM
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public bool Removed { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}