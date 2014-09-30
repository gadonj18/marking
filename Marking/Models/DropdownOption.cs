using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class DropdownOption : _BaseModel
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int OptionOrder { get; set; }
        public int CriterionID { get; set; }
        public virtual Criterion Criterion { get; set; }
    }
}