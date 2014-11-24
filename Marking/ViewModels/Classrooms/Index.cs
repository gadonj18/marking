using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marking.ViewModels.Classroom
{
    public class Index
    {
        public int Year { get; set; }
        public List<IndexClassroom> Classrooms { get; set; }

        public class IndexClassroom
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Grade { get; set; }
            public IEnumerable<IndexAssessment> Assessments { get; set; }
            public List<SelectListItem> YearList { get; set; }
        }
        
        public class IndexAssessment
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Description { get; set; }
        }
    }
}