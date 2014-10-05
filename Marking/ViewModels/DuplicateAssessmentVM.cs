using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class DuplicateAssessmentVM
    {
        public int ClassroomID { get; set; }
        public string ClassroomTitle { get; set; }
        public int Grade { get; set; }
        public List<int> FilterYears { get; set; }
        public List<int> FilterGrades { get; set; }
        public IQueryable<Classroom> FilterClassrooms { get; set; }
        public IQueryable<Assessment> Assessments { get; set; }

        public class Classroom
        {
            public int ID { get; set; }
            public string Title { get; set; }
        }

        public class Assessment
        {
            public int AssessmentID { get; set; }
            public int ClassroomID { get; set; }
            public string ClassrooomTitle { get; set; }
            public int Year { get; set; }
            public int Grade { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
        }
    }
}