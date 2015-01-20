using VMs = Marking.ViewModels.Classroom;
using Models = Marking.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Web.Mvc;
using System;
using System.Data.Entity;

namespace Marking.DAL.Mapping
{
    public class ClassroomMapper
    {
        public IndexMapper Index;
        public CreateEditMapper CreateEdit;

        public ClassroomMapper(MarkingContext db)
        {
            Index = new IndexMapper(db);
            CreateEdit = new CreateEditMapper(db);
        }

        public class IndexMapper
        {
            private MarkingContext db;

            public IndexMapper(MarkingContext db)
            {
                this.db = db;
            }

            public VMs.Index ListByYear(int year)
            {
                var MaxYear = db.Classrooms.Max(x => x.Year);
                if (MaxYear == null)
                {
                    MaxYear = DateTime.Now.Year;
                    if (DateTime.Now.Month < 7) MaxYear--;
                }
                var MinYear = db.Classrooms.Min(x => x.Year);
                var years = new List<SelectListItem>();
                for (var i = MaxYear; i >= MinYear; i--)
                {
                    years.Add(new SelectListItem() { Text = i.ToString() + " / " + (i + 1).ToString(), Value = i.ToString(), Selected = (i == year) });
                }
                return new VMs.Index()
                {
                    Year = year,
                    Years = years,
                    Classrooms = (from classroom in db.Classrooms
                                  where classroom.Year == year
                                  select new VMs.Index.IndexClassroom
                                  {
                                      ID = classroom.ID,
                                      Title = classroom.Title,
                                      Grade = classroom.Grade,
                                      Assessments = (from assess in db.Assessments
                                                     where assess.ClassroomID == classroom.ID
                                                     select new VMs.Index.IndexAssessment
                                                     {
                                                         ID = assess.ID,
                                                         Title = assess.Title,
                                                         Subtitle = assess.Subtitle,
                                                         Description = assess.Description
                                                     }).ToList()
                                  }).ToList()
                };
            }
        }

        public class CreateEditMapper
        {
            private MarkingContext db;

            public CreateEditMapper(MarkingContext db)
            {
                this.db = db;
            }

            public VMs.CreateEdit CreateNew()
            {
                VMs.CreateEdit vm = new VMs.CreateEdit();
                return Prep(vm);
            }

            public VMs.CreateEdit CreateFromID(int id)
            {
                Models.Classroom classroom = db.Classrooms.Find(id);
                if (classroom == null) { return null; }
                VMs.CreateEdit vm = CreateFrom(classroom);
                return vm;
            }

            public VMs.CreateEdit CreateFrom(Models.Classroom classroom)
            {
                Mapper.CreateMap<Models.Classroom, VMs.CreateEdit>();
                VMs.CreateEdit vm = Mapper.Map<Models.Classroom, VMs.CreateEdit>(classroom);
                return Prep(vm);
            }

            public VMs.CreateEdit Prep(VMs.CreateEdit vm)
            {
                if (vm.GradeList == null)
                {
                    vm.GradeList = new List<SelectListItem>();
                }
                List<string> grades = new List<string>() {
                    "Jr. Kindergarten", "Sr. Kindergarten", "Grade 1", "Grade 2", "Grade 3", 
                    "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", 
                    "Grade 9", "Grade 10", "Grade 11", "Grade 12"
                };
                vm.GradeList.Add(new SelectListItem { Selected = (vm.Grade == null), Value = "", Text = "" });
                foreach (var grade in grades)
                {
                    vm.GradeList.Add(new SelectListItem { Selected = (vm.Grade == grade), Value = grade, Text = grade });
                }

                //Year list should be the earliest recorded year minus one to the current year plus 1
                if (vm.YearList == null)
                {
                    vm.YearList = new List<SelectListItem>();
                }
                var currYear = DateTime.Now.Year;
                if (DateTime.Now.Month < 7) currYear--;
                var MinYear = (from classroom in db.Classrooms select classroom.Year).Min();
                if (MinYear == null)
                {
                    MinYear = currYear;
                }
                MinYear--;
                for (int year = currYear + 1; year >= MinYear; year--)
                {
                    vm.YearList.Add(new SelectListItem { Selected = (year == currYear), Value = year.ToString(), Text = year.ToString() + " / " + (year + 1).ToString() });
                }
                return vm;
            }

            public void ApplyChanges(VMs.CreateEdit vm)
            {
                Models.Classroom classroom = (vm.ID == null ? new Models.Classroom() : db.Classrooms.Find(vm.ID));
                Mapper.CreateMap<VMs.CreateEdit, Models.Classroom>();
                Mapper.Map<VMs.CreateEdit, Models.Classroom>(vm, classroom);
                if (vm.ID == null)
                {
                    db.Classrooms.Add(classroom);
                }
                else
                {
                    db.Entry(classroom).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
    }
}