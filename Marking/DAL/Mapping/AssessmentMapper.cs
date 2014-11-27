using VMs = Marking.ViewModels.Assessments;
using Models = Marking.Models;
using AutoMapper;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
namespace Marking.DAL.Mapping
{
    public class AssessmentMapper
    {
        public CreateEditMapper CreateEdit;

        public AssessmentMapper(MarkingContext db)
        {
            CreateEdit = new CreateEditMapper(db);
        }

        public class CreateEditMapper
        {
            private MarkingContext db;

            public CreateEditMapper(MarkingContext db)
            {
                this.db = db;
            }

            public VMs.CreateEdit CreateNew(int classroomID)
            {
                Models.Classroom classroom = db.Classrooms.Find(classroomID);
                if (classroom == null) return null;
                VMs.CreateEdit vm = new VMs.CreateEdit();
                vm.ClassroomTitle = classroom.Title;
                vm.Grade = classroom.Grade;
                vm.ClassroomID = classroom.ID;
                return Prep(vm);
            }

            public VMs.CreateEdit CreateFromID(int assessmentID)
            {
                var vm = (from assessment in db.Assessments
                          from classroom in db.Classrooms
                          where assessment.ID == assessmentID
                            && assessment.ClassroomID == classroom.ID
                          select new VMs.CreateEdit
                          {
                              ClassroomTitle = classroom.Title,
                              Grade = classroom.Grade,
                              AssessmentTitle = assessment.Title,
                              AssessmentSubtitle = assessment.Subtitle,
                              Description = assessment.Description,
                              GroupWork = assessment.GroupWork,
                              DateDue = assessment.DateDue,
                              Attachments = from attachment in db.AssessmentAttachments
                                            where attachment.AssessmentID == assessment.ID
                                            select new VMs.CreateEdit.Attachment
                                            {
                                                ID = attachment.ID,
                                                Title = attachment.Title,
                                                Filename = attachment.Filename,
                                                Removed = false,
                                                DateCreated = attachment.DateCreated
                                            },
                              Notes = from note in db.Notes
                                      where note.ParentModel == "Assessment"
                                        && note.ParentID == assessment.ID
                                      select new VMs.CreateEdit.Note
                                      {
                                          ID = note.ID,
                                          ParentID = note.ParentID,
                                          ParentModel = note.ParentModel,
                                          Text = note.Text,
                                          DateCreated = note.DateCreated
                                      },
                              Criteria = from criterion in db.Criteria
                                         where criterion.AssessmentID == assessment.ID
                                         select new VMs.CreateEdit.Criterion
                                         {
                                             ID = criterion.ID,
                                             OldFieldType = criterion.FieldType,
                                             FieldType = criterion.FieldType,
                                             Label = criterion.Label,
                                             FieldOrder = criterion.FieldOrder,
                                             Removed = false,
                                             Options = from option in db.DropdownOptions
                                                       where option.CriterionID == criterion.ID
                                                       select new VMs.CreateEdit.DropdownOption
                                                       {
                                                           ID = option.ID,
                                                           Key = option.Key,
                                                           Value = option.Value,
                                                           OptionOrder = option.OptionOrder,
                                                           Removed = false
                                                       }
                                         }

                          }).SingleOrDefault();
                return Prep(vm);
            }

            public VMs.CreateEdit Prep(VMs.CreateEdit vm)
            {
                return vm;
            }

            public void ApplyChanges(VMs.CreateEdit vm)
            {
                Models.Assessment assessment = (vm.ID == null ? new Models.Assessment() : db.Assessments.Find(vm.ID));
                Mapper.CreateMap<VMs.CreateEdit, Models.Assessment>()
                            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.AssessmentTitle))
                            .ForMember(dest => dest.Subtitle, opt => opt.MapFrom(src => src.AssessmentSubtitle))
                            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
                Mapper.CreateMap<VMs.CreateEdit.Note, Models.Note>();
                Mapper.CreateMap<VMs.CreateEdit.Criterion, Models.Criterion>();
                Mapper.CreateMap<VMs.CreateEdit.DropdownOption, Models.DropdownOption>();
                Mapper.Map<VMs.CreateEdit, Models.Assessment>(vm, assessment);

                assessment.Attachments = new List<Models.AssessmentAttachment>();
                foreach (var attachmentVM in vm.NewAttachments)
                {
                    attachmentVM.ContentType = attachmentVM.File.ContentType;
                    attachmentVM.Filename = attachmentVM.File.FileName;
                    attachmentVM.FilenameInternal = db.UploadAttachment(attachmentVM.File);
                    Mapper.CreateMap<VMs.CreateEdit.NewAttachment, Models.AssessmentAttachment>();
                    Models.AssessmentAttachment attachment = Mapper.Map<VMs.CreateEdit.NewAttachment, Models.AssessmentAttachment>(attachmentVM);
                    assessment.Attachments.Add(attachment);
                }
                if (vm.ID == null)
                {
                    db.Assessments.Add(assessment);
                }
                else
                {
                    db.Entry(assessment).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
    }
}