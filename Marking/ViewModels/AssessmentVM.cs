using Marking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class AssessmentVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public bool GroupWork { get; set; }
        public DateTime? DateDue { get; set; }
        public IEnumerable<CriterionVM> Criteria { get; set; }
        public ICollection<NoteVM> Notes { get; set; }
        public ICollection<AttachmentVM> Attachments { get; set; }
    }
}