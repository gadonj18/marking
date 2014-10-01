using Marking.DAL;
using Marking.Models;
using Marking.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Marking.Controllers
{
    public class MarksController : Controller
    {
        private MarkingContext db = new MarkingContext();

        public ActionResult ListForAssessment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = (from assess in db.Assessments
                        where assess.ID == id
                        select new StudentListVM
                        {
                            ClassroomTitle = assess.Classroom.Title,
                            Grade = assess.Classroom.Grade,
                            AssessmentTitle = assess.Title,
                            AssessmentSubtitle = assess.Subtitle,
                            Students = from enroll in assess.Classroom.Enrollments
                                       select new StudentListVM.StudentListVMStudent
                                       {
                                           StudentID = enroll.StudentID,
                                           StudentName = enroll.Student.FirstName + " " + enroll.Student.LastName,
                                           Marks = from criteria in assess.Criteria
                                                   orderby criteria.FieldOrder
                                                   select new StudentListVM.StudentListVMMark
                                                   {
                                                       Value = criteria.Marks.FirstOrDefault(x => x.StudentID == enroll.StudentID).Value,
                                                       StudentID = enroll.StudentID,
                                                       CriterionID = criteria.ID,
                                                       FieldType = criteria.FieldType,
                                                       Label = criteria.Label,
                                                       Options = from option in criteria.Options
                                                                 select new StudentListVM.StudentListVMOption
                                                                 {
                                                                     Key = option.Key,
                                                                     Value = option.Value
                                                                 }
                                                   }
                                       }
                        }).FirstOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult UpdateMark(int CriterionID, int StudentID, string Value)
        {
            if (!Request.IsAjaxRequest())
            {
                return Json(new { code = "FAIL" });
            }

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Mark mark = (from oldMark in db.Marks
                                 where oldMark.CriterionID == CriterionID
                                 select oldMark).FirstOrDefault();
                    if (mark == null)
                    {
                        mark = new Mark()
                        {
                            CriterionID = CriterionID,
                            StudentID = StudentID,
                            Value = Value
                        };
                        db.Entry(mark).State = EntityState.Added;
                        db.Marks.Add(mark);
                    }
                    else
                    {
                        db.Entry(mark).State = EntityState.Modified;
                        mark.Value = Value;
                    }
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json(new { code = "PASS" });
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return Json(new { code = "FAILSTOP" });
                }
            }
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