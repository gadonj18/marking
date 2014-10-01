using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class ClassroomListVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Grade { get; set; }
        public IEnumerable<ClassroomListVMAssessment> Assessments { get; set; }

        public class ClassroomListVMAssessment
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Description { get; set; }
        }
    }
}