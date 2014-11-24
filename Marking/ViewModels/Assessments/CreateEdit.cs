using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
namespace Marking.ViewModels.Assessments
{
    public class CreateEdit
    {
        public int? ID { get; set; }

        public int ClassroomID { get; set; }
        public string ClassroomTitle { get; set; }
        public string Grade { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage="Required")]
        public string AssessmentTitle { get; set; }

        [DisplayName("Subtitle")]
        public string AssessmentSubtitle { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Group Work?")]
        public bool GroupWork { get; set; }

        [DisplayName("Date Due")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateDue { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }
        public IEnumerable<NewAttachment> NewAttachments { get; set; }
        public IEnumerable<Note> Notes { get; set; }
        public IEnumerable<Criterion> Criteria { get; set; }

        public class Attachment
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Required")]
            public string Title { get; set; }
            public string Filename { get; set; }
            public string FilenameInternal { get; set; }
            public bool Removed { get; set; }
            [Required(ErrorMessage = "Required")]
            public DateTime? DateCreated { get; set; }
        }

        public class NewAttachment
        {
            [Required(ErrorMessage = "Required")]
            public string Title { get; set; }
            [Required(ErrorMessage = "Required")]
            public HttpPostedFileBase File { get; set; }
            public string Filename { get; set; }
            public string FilenameInternal { get; set; }
            public string ContentType { get; set; }
        }

        public class Note
        {
            public int ID { get; set; }
            public int ParentID { get; set; }
            public string ParentModel { get; set; }
            [Required(ErrorMessage = "Required")]
            [DataType(DataType.MultilineText)]
            public string Text { get; set; }
            public bool Removed { get; set; }
            public DateTime? DateCreated { get; set; }

            public Note()
            {
                ParentModel = "Assessment";
            }
        }

        public class Criterion
        {
            public int ID { get; set; }
            public string OldFieldType { get; set; }
            [Required(ErrorMessage = "Required")]
            public string FieldType { get; set; }
            [Required(ErrorMessage = "Required")]
            public string Label { get; set; }
            public int FieldOrder { get; set; }
            public bool Removed { get; set; }
            public IEnumerable<DropdownOption> Options { get; set; }
        }

        public class DropdownOption
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Required")]
            public string Key { get; set; }
            [Required(ErrorMessage = "Required")]
            public string Value { get; set; }
            public int OptionOrder { get; set; }
            public bool Removed { get; set; }
        }
    }
}