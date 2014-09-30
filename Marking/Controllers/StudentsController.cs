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
using AutoMapper;
using Marking.ViewModels;

namespace Marking.Controllers
{
    public class StudentsController : Controller
    {
        private MarkingContext db = new MarkingContext();

        // GET: Students/ListForAssessment/1
        public ActionResult StudentList(int? id)
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
                                        StudentName = enroll.Student.FirstName + " " + enroll.Student.LastName,
                                        Marks = from criteria in assess.Criteria
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

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateCreated,DateModified")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateCreated,DateModified")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
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
