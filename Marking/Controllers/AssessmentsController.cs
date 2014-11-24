using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Marking.DAL;
using Marking.Models;
using Marking.ViewModels;
using AutoMapper;
using Marking.Library;
using Marking.DAL.Mapping;
using VMs = Marking.ViewModels.Assessments;

namespace Marking.Controllers
{
    public class AssessmentsController : Controller
    {
        private MarkingContext db;
        public AssessmentMapper vmMapper;

        public AssessmentsController()
        {
            db = new MarkingContext();
            vmMapper = new AssessmentMapper(db);
        }

        // /Classrooms/1/Create
        // /Classrooms/1/Edit/1
        [Route("Classrooms/{classroomID:int}/Create")]
        [Route("Classrooms/{classroomID:int}/Edit/{id:int}")]
        public ActionResult CreateEdit(int classroomID, int? id)
        {
            VMs.CreateEdit vm;
            if (id == null)
            {
                vm = vmMapper.CreateEdit.CreateNew(classroomID);
            }
            else
            {
                vm = vmMapper.CreateEdit.CreateFromID((int)id);
            }
            if (vm == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(vm);
        }

        [Route("Classrooms/{classroomID:int}/CreateEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(int classroomID, VMs.CreateEdit vm)
        {
            if (classroomID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(classroomID);
            if (classroom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vm.ClassroomTitle = classroom.Title;
            vm.Grade = classroom.Grade;

            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        /*Mapper.CreateMap<AssessmentCreateEditVM, Assessment>()
                            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.AssessmentTitle))
                            .ForMember(dest => dest.Subtitle, opt => opt.MapFrom(src => src.AssessmentSubtitle))
                            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
                        Mapper.CreateMap<AssessmentCreateEditVM.Note, Note>();
                        Mapper.CreateMap<AssessmentCreateEditVM.Criterion, Criterion>();
                        Mapper.CreateMap<AssessmentCreateEditVM.DropdownOption, DropdownOption>();
                        Assessment assessment = Mapper.Map<AssessmentCreateEditVM, Assessment>(vm);
                        assessment.ClassroomID = classroom.ID;

                        assessment.Attachments = new List<AssessmentAttachment>();
                        foreach (var attachmentVM in vm.NewAttachments)
                        {
                            attachmentVM.ContentType = attachmentVM.File.ContentType;
                            attachmentVM.Filename = attachmentVM.File.FileName;
                            attachmentVM.FilenameInternal = db.UploadAttachment(attachmentVM.File);
                            Mapper.CreateMap<AssessmentCreateEditVM.NewAttachment, AssessmentAttachment>();
                            AssessmentAttachment attachment = Mapper.Map<AssessmentCreateEditVM.NewAttachment, AssessmentAttachment>(attachmentVM);
                            assessment.Attachments.Add(attachment);
                        }

                        db.Entry(assessment).State = EntityState.Added;
                        db.Assessments.Add(assessment);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        TempData["Flash"] = "Assessment successfully created";
                        TempData["FlashType"] = "GreenFlash";*/
                        return RedirectToAction("Index", "Classrooms");
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        TempData["Flash"] = "Unexpected error creating new assessment";
                        return RedirectToAction("Create");
                    }
                }
            }
            return View(vm);
        }

        /*public ActionResult Duplicate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vm = (from classroom in db.Classrooms
                      where classroom.ID == id
                      select new DuplicateAssessmentVM
                      {
                          ClassroomID = classroom.ID,
                          ClassroomTitle = classroom.Title,
                          Grade = classroom.Grade,
                          FilterYears = (List<int>)db.Classrooms.OrderBy(x => x.Year).Select(x => x.Year).Distinct<int>(),
                          FilterGrades = (List<int>)db.Classrooms.OrderBy(x => x.Grade).Select(x => x.Grade).Distinct<int>(),
                          FilterClassrooms = from classroom3 in db.Classrooms
                                             select new DuplicateAssessmentVM.Classroom {
                                                 ID = classroom3.ID,
                                                 Title = classroom3.Title
                                             },
                          Assessments = from assessment in db.Assessments
                                        from classroom2 in db.Classrooms
                                        where assessment.ClassroomID == classroom2.ID
                                        select new DuplicateAssessmentVM.Assessment
                                        {
                                            AssessmentID = assessment.ID,
                                            ClassroomID = classroom2.ID,
                                            ClassrooomTitle = classroom2.Title,
                                            Year = classroom2.Year,
                                            Grade = classroom2.Grade,
                                            Title = assessment.Title,
                                            Subtitle = assessment.Subtitle
                                        }
                      }).SingleOrDefault();
            if (vm == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duplicate(int? AssessmentID, int? ClassroomID)
        {
            if (AssessmentID == null || ClassroomID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assessment assessment = db.Assessments.Find(AssessmentID);
            Classroom classroom = db.Classrooms.Find(ClassroomID);
            if (assessment == null || classroom == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<Assessment, Assessment>()
                .ForMember(dest => dest.ID, opt => opt.UseValue(0))
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            Mapper.CreateMap<Criterion, Criterion>()
                .ForMember(dest => dest.ID, opt => opt.UseValue(0));
            Mapper.CreateMap<DropdownOption, DropdownOption>()
                .ForMember(dest => dest.ID, opt => opt.UseValue(0));
            Assessment newAssessment = Mapper.Map<Assessment, Assessment>(assessment);
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Assessments.Add(newAssessment);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    TempData["Flash"] = "Assessment successfully copied";
                    TempData["FlashType"] = "GreenFlash";
                    return RedirectToAction("Index", "Classrooms");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    TempData["Flash"] = "Unexpected error while copying assessment";
                    return RedirectToAction("AddAssessment", "Classrooms", new { id = ClassroomID, error = e.InnerException.Message });
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vm = (from assessment in db.Assessments
                      from classroom in db.Classrooms
                      where assessment.ID == id
                        && assessment.ClassroomID == classroom.ID
                      select new AssessmentCreateEditVM
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
                                        select new AssessmentCreateEditVM.Attachment
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
                                  select new AssessmentCreateEditVM.Note
                                  {
                                      ID = note.ID,
                                      ParentID = note.ParentID,
                                      ParentModel = note.ParentModel,
                                      Text = note.Text,
                                      DateCreated = note.DateCreated
                                  },
                          Criteria = from criterion in db.Criteria
                                     where criterion.AssessmentID == assessment.ID
                                     select new AssessmentCreateEditVM.Criterion
                                     {
                                         ID = criterion.ID,
                                         OldFieldType = criterion.FieldType,
                                         FieldType = criterion.FieldType,
                                         Label = criterion.Label,
                                         FieldOrder = criterion.FieldOrder,
                                         Removed = false,
                                         Options = from option in db.DropdownOptions
                                                   where option.CriterionID == criterion.ID
                                                   select new AssessmentCreateEditVM.DropdownOption
                                                   {
                                                       ID = option.ID,
                                                       Key = option.Key,
                                                       Value = option.Value,
                                                       OptionOrder = option.OptionOrder,
                                                       Removed = false
                                                   }
                                     }

                      }).SingleOrDefault();
            if (vm == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult NewCriterionRow()
        {
            if (!Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }
            AssessmentCreateEditVM.Criterion criterion = new AssessmentCreateEditVM.Criterion();
            criterion.FieldOrder = -1;
            return PartialView("~/Views/Criteria/_CriterionVM.cshtml", criterion);
        }

        [HttpPost]
        public ActionResult NewOptionRow(string CriterionIndex)
        {
            if (!Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }
            AssessmentCreateEditVM.DropdownOption option = new AssessmentCreateEditVM.DropdownOption();
            option.OptionOrder = -1;
            ViewData.TemplateInfo.HtmlFieldPrefix = "Criteria[" + CriterionIndex + "]";
            return PartialView("~/Views/DropdownOptions/_DropdownOptionVM.cshtml", option);
        }

        [HttpPost]
        public ActionResult NewNote()
        {
            if (!Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Notes/_NewNote.cshtml", new AssessmentCreateEditVM.Note());
        }

        [HttpPost]
        public ActionResult NewAttachment()
        {
            if (!Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Attachments/_NewAttachment.cshtml", new AssessmentCreateEditVM.NewAttachment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, AssessmentVM assessmentVM)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assessment assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                Mapper.CreateMap<DropdownOptionVM, DropdownOption>();
                Mapper.CreateMap<CriterionVM, Criterion>()
                    .ForMember(desc => desc.Marks, opt => opt.Ignore())
                    .ForMember(desc => desc.Options, opt => opt.Ignore());
                Mapper.CreateMap<AssessmentVM, Assessment>()
                    .ForMember(desc => desc.Criteria, opt => opt.Ignore())
                    .ForMember(desc => desc.Attachments, opt => opt.Ignore())
                    .ForMember(desc => desc.Notes, opt => opt.Ignore());

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        IList<Criterion> criteria = assessment.Criteria;
                        Mapper.Map<AssessmentVM, Assessment>(assessmentVM, assessment);
                        assessment.Criteria = criteria;

                        foreach (var criterion in assessmentVM.Criteria)
                        {
                            Criterion targetCriteria = assessment.Criteria.SingleOrDefault(c => c.ID.Equals(criterion.ID));
                            if (targetCriteria == null)
                            {
                                targetCriteria = Mapper.Map<CriterionVM, Criterion>(criterion);
                                targetCriteria.AssessmentID = assessment.ID;
                                db.Entry(targetCriteria).State = EntityState.Added;
                                db.Criteria.Add(targetCriteria);
                            }
                            else
                            {
                                IList<DropdownOption> options = targetCriteria.Options;
                                Mapper.Map<CriterionVM, Criterion>(criterion, targetCriteria);
                                targetCriteria.Options = options;
                                db.Entry(targetCriteria).State = EntityState.Modified;
                            }

                            foreach (var option in criterion.Options)
                            {
                                DropdownOption targetOption = targetCriteria.Options.SingleOrDefault(c => c.ID.Equals(option.ID));
                                if (targetOption == null)
                                {
                                    targetOption = Mapper.Map<DropdownOptionVM, DropdownOption>(option);
                                    targetOption.CriterionID = targetCriteria.ID;
                                    db.Entry(targetOption).State = EntityState.Added;
                                    db.DropdownOptions.Add(targetOption);
                                }
                                else
                                {
                                    Mapper.Map<DropdownOptionVM, DropdownOption>(option, targetOption);
                                    db.Entry(targetOption).State = EntityState.Modified;
                                }
                            }
                        }
                        db.Entry(assessment).State = EntityState.Modified;


                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        TempData["Flash"] = "Assessment successfully saved";
                        TempData["FlashType"] = "GreenFlash";

                        return RedirectToAction("Index", "Classrooms", null);
                    }
                    catch (Exception)
                    {
                        TempData["Flash"] = "Unexpected error while saving assessment";
                        dbContextTransaction.Rollback();
                        View(assessmentVM);
                    }
                }
            }
            return View(assessmentVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
