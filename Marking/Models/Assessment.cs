using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class Assessment : _BaseModel
    {
        public int ID { get; set; }
        [Display(Name = "Assignment Title")]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Groupwork?")]
        public bool GroupWork { get; set; }
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateDue { get; set; }

        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual IList<Criterion> Criteria { get; set; }

        public static Assessment Copy(Assessment assessment)
        {
            Mapper.CreateMap<Assessment, Assessment>()
                .ForMember(dest => dest.ID, opt => opt.UseValue(0))
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            Mapper.CreateMap<Criterion, Criterion>()
                .ForMember(dest => dest.ID, opt => opt.UseValue(0));
            Mapper.CreateMap<DropdownOption, DropdownOption>()
                .ForMember(dest => dest.ID, opt => opt.UseValue(0));
            Assessment newAssessment = Mapper.Map<Assessment, Assessment>(assessment);
            return newAssessment;
        }
    }
}