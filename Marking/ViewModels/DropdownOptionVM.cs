using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public class DropdownOptionVM
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int OptionOrder { get; set; }
        public bool Removed { get; set; }
        public bool NewOption { get; set; }
    }
}