using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class AssessmentCreateEditVM
    {
        public string ClassroomTitle { get; set; }
        public int Grade { get; set; }
        public string AssessmentTitle { get; set; }
        public string AssessmentSubtitle { get; set; }
        public string Description { get; set; }
        public bool GroupWork { get; set; }
        public DateTime? DateDue { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Note> Notes { get; set; }
        public List<Criterion> Criteria { get; set; }

        public class Attachment
        {
            public int ID { get; set; }
            public Marking.Models.AttachmentTypes Type { get; set; }
            public string Title { get; set; }
            public string Filename { get; set; }
            public bool Removed { get; set; }
            public HttpPostedFileBase File { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public class Note
        {
            public int ID { get; set; }
            public string Text { get; set; }
            public bool Removed { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public class Criterion
        {
            public int ID { get; set; }
            public Marking.Models.FieldTypes? OldFieldType { get; set; }
            public Marking.Models.FieldTypes? FieldType { get; set; }
            public string Label { get; set; }
            public int FieldOrder { get; set; }
            public bool Removed { get; set; }
            public List<DropdownOption> Options { get; set; }
        }

        public class DropdownOption
        {
            public int ID { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
            public int OptionOrder { get; set; }
            public bool Removed { get; set; }
            public bool NewOption { get; set; }
        }
    }
}