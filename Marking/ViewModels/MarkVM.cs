using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class MarkVM
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public CriterionVM Criterion { get; set; }
        public StudentVM Student { get; set; }
    }
}