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

namespace Marking.Controllers
{
    public class ClassroomsController : Controller
    {
        private MarkingContext db = new MarkingContext();

        public ActionResult Index()
        {
            return View(db.Classrooms.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassroomVM classroomVM)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Mapper.CreateMap<ClassroomVM, Classroom>();
                        Classroom classroom = Mapper.Map<ClassroomVM, Classroom>(classroomVM);
                        db.Classrooms.Add(classroom);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        TempData["Flash"] = "Classroom successfully created";
                        TempData["FlashType"] = "GreenFlash";
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        TempData["Flash"] = "Unexpected error creating classroom";
                    }
                }
            }
            return View(classroomVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, ClassroomVM classroomVM)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Mapper.CreateMap<ClassroomVM, Classroom>()
                            .ForMember(dest => dest.ID, opt => opt.Ignore())
                            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                            .ForMember(dest => dest.Assessments, opt => opt.Ignore())
                            .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
                            .ForMember(dest => dest.Notes, opt => opt.Ignore());
                        Mapper.Map<ClassroomVM, Classroom>(classroomVM, classroom);

                        db.Entry(classroom).State = EntityState.Modified;
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        TempData["Flash"] = "Classroom successfully saved";
                        TempData["FlashType"] = "GreenFlash";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        TempData["Flash"] = "Unexpected error saving classroom: " + ex.Message;
                    }
                }
            }
            return View(classroom);
        }

        public ActionResult AddAssessment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }

            List<Assessment> assessments = db.Assessments.Where<Assessment>(x => x.ClassroomID != id).ToList<Assessment>();
            List<Classroom> classrooms = db.Classrooms.ToList<Classroom>();

            ViewBag.Years = classrooms.OrderBy(x => x.Year).Select(x => x.Year).Distinct<int>();
            ViewBag.Grades = classrooms.OrderBy(x => x.Grade).Select(x => x.Grade).Distinct<int>();
            ViewBag.Assessments = assessments;
            ViewBag.Classrooms = classrooms;

            return View(classroom);
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
