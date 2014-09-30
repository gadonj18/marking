using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class ClassroomVM
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Grade { get; set; }
        [Required]
        public int Year { get; set; }
        public IEnumerable<AssessmentVM> Assessments { get; set; }
        public IEnumerable<AttachmentVM> Attachments { get; set; }
        public IEnumerable<NoteVM> Notes { get; set; }
    }
}