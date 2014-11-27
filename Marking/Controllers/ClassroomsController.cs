using System;
using System.Net;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Marking.DAL;
using Marking.DAL.Mapping;
using Marking.Models;
using VMs = Marking.ViewModels.Classroom;
using AutoMapper;
using Marking.ViewModels.Classroom;

namespace Marking.Controllers
{
    public class ClassroomsController : Controller
    {
        private MarkingContext db;
        private ClassroomMapper vmMapper;

        public ClassroomsController()
        {
            db = new MarkingContext();
            vmMapper = new ClassroomMapper(db);
        }

        // /Classrooms
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Classrooms", new { year = DateTime.Now.Year });
        }

        // /Classrooms/2013
        [Route("Classrooms/{year:int}")]
        public ActionResult Index(int year)
        {
            var vm = vmMapper.Index.ListByYear((int)year);
            return View(vm);
        }

        // /Classrooms/Create
        // /Classrooms/Edit/1
        [Route("Classrooms/Create")]
        [Route("Classrooms/Edit/{id}")]
        public ActionResult CreateEdit(int? id)
        {
            VMs.CreateEdit vm;
            if (id == null)
            {
                vm = vmMapper.CreateEdit.CreateNew();
            }
            else
            {
                vm = vmMapper.CreateEdit.CreateFromID((int)id);
            }
            if (vm == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // /Classrooms/CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Classrooms/CreateEdit")]
        public ActionResult CreateEdit(VMs.CreateEdit vm)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        vmMapper.CreateEdit.ApplyChanges(vm);
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
            return View(vmMapper.CreateEdit.Prep(vm));
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
