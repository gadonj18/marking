using Marking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class StudentListVM
    {
        public string ClassroomTitle { get; set; }
        public int Grade { get; set; }
        public string AssessmentTitle { get; set; }
        public string AssessmentSubtitle { get; set; }
        public IEnumerable<StudentListVMStudent> Students { get; set; }

        public class StudentListVMStudent
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public IEnumerable<StudentListVMMark> Marks { get; set; }
        }

        public class StudentListVMMark
        {
            public string Value { get; set; }
            public int StudentID { get; set; }
            public int CriterionID { get; set; }
            public string FieldType { get; set; }
            public string Label { get; set; }
            public IEnumerable<StudentListVMOption> Options { get; set; }
        }

        public class StudentListVMOption
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}