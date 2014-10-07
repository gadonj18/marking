using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.Models
{
    public class Note : _BaseModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string ParentModel { get; set; }
        public string Text { get; set; }
    }
}