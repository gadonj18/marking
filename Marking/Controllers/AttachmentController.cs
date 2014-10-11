using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Marking.Controllers
{
    public class AttachmentController : Controller
    {
        public FilePathResult Download(int? id)
        {

            return new FilePathResult("~/Content/uploads/", System.Net.Mime.MediaTypeNames.Application.Octet);
        }
    }
}