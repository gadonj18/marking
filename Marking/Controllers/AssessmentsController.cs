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

namespace Marking.Controllers
{
    public class AssessmentsController : Controller
    {
        private MarkingContext db = new MarkingContext();

        public ActionResult Index()
        {
            var assessments = db.Assessments.Include(a => a.Classroom);
            return View(assessments.ToList());
        }        

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssessmentCreateEditVM vm = new AssessmentCreateEditVM();
            vm.ClassroomTitle = classroom.Title;
            vm.Grade = classroom.Grade;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id, Assessment Assessment)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assessment.ClassroomID = classroom.ID;

            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Assessments.Add(Assessment);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        TempData["Flash"] = "Assessment successfully created";
                        TempData["FlashType"] = "GreenFlash";
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

            ViewBag.Classroom = classroom;
            return View();
        }

        public ActionResult Duplicate(int? id)
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
            Assessment assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Assessment, AssessmentVM>();
            Mapper.CreateMap<Criterion, CriterionVM>()
                .ForMember(dest => dest.Marks, opt => opt.Ignore())
                .ForMember(dest => dest.OldFieldType, opt => opt.MapFrom(src => src.FieldType));
            Mapper.CreateMap<DropdownOption, DropdownOptionVM>();
            Mapper.CreateMap<Note, NoteVM>();
            Mapper.CreateMap<Attachment, AttachmentVM>();
            AssessmentVM assessmentVM = Mapper.Map<Assessment, AssessmentVM>(assessment);
            return View(assessmentVM);
        }

        [HttpPost]
        public ActionResult NewCriterionRow()
        {
            if (!Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }
            CriterionVM criterion = new CriterionVM();
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
            DropdownOptionVM option = new DropdownOptionVM();
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
            return PartialView("~/Views/Notes/_NewNote.cshtml", new NoteVM());
        }

        [HttpPost]
        public ActionResult NewAttachment()
        {
            if (!Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Attachments/_NewAttachment.cshtml", new AttachmentVM());
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
                    catch (Exception e)
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
        }
    }
}
