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

        public ActionResult Index(int? year = null)
        {
            if (year == null) year = DateTime.Now.Year;
            var classrooms = (from classroom in db.Classrooms
                             where classroom.Year == year
                             select new ClassroomListVM
                             {
                                 ID = classroom.ID,
                                 Title = classroom.Title,
                                 Grade = classroom.Grade,
                                 Assessments = from assess in db.Assessments
                                               where assess.ClassroomID == classroom.ID
                                               select new ClassroomListVM.ClassroomListVMAssessment
                                               {
                                                   ID = assess.ID,
                                                   Title = assess.Title,
                                                   Subtitle = assess.Subtitle,
                                                   Description = assess.Description
                                               }
                             }).ToList();
            return View(classrooms);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassroomCreateEditVM classroomVM)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Mapper.CreateMap<ClassroomCreateEditVM, Classroom>();
                        Classroom classroom = Mapper.Map<ClassroomCreateEditVM, Classroom>(classroomVM);
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
            Mapper.CreateMap<Classroom, ClassroomCreateEditVM>();
            ClassroomCreateEditVM classroomVM = Mapper.Map<Classroom, ClassroomCreateEditVM>(classroom);
            return View(classroomVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, ClassroomCreateEditVM classroomVM)
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
                        Mapper.CreateMap<ClassroomCreateEditVM, Classroom>();
                        Mapper.Map<ClassroomCreateEditVM, Classroom>(classroomVM, classroom);

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
