using Marking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class CriterionVM
    {
        public int ID { get; set; }
        public FieldTypes? OldFieldType { get; set; }
        public FieldTypes? FieldType { get; set; }
        public string Label { get; set; }
        public IEnumerable<DropdownOptionVM> Options { get; set; }
        public IEnumerable<MarkVM> Marks { get; set; }
        public int FieldOrder { get; set; }
        public bool Removed { get; set; }
        public bool NewCriterion { get; set; }

        public CriterionVM()
        {
            Options = new List<DropdownOptionVM>();
        }
    }
}