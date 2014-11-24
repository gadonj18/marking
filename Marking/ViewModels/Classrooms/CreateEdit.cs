using Marking.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marking.ViewModels.Classroom
{
    public class CreateEdit
    {
        public int? ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Grade { get; set; }

        public List<SelectListItem> GradeList { get; set; }

        [Required]
        public int Year { get; set; }

        public List<SelectListItem> YearList { get; set; }
    }
}