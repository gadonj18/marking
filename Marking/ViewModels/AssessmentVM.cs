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
        public IEnumerable<NoteVM> Notes { get; set; }
        public IEnumerable<AttachmentVM> Attachments { get; set; }

        public AssessmentVM()
        {
            Criteria = new List<CriterionVM>();
            Notes = new List<NoteVM>();
            Attachments = new List<AttachmentVM>();
        }
    }
}