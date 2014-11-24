using Marking.DAL;
using Marking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;

namespace Marking.Controllers
{
    public class AttachmentsController : Controller
    {
        private MarkingContext db = new MarkingContext();

        public ActionResult Download(int? id, string type)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _BaseAttachment attachment;
            switch (type)
            {
                case "Assessment":
                    attachment = db.AssessmentAttachments.Find(id);
                    break;
                case "Classroom":
                    attachment = db.ClassroomAttachments.Find(id);
                    break;
                case "Student":
                    attachment = db.StudentAttachments.Find(id);
                    break;
                default:
                    throw new Exception("Missing/Invalid attachment type");
            }
            ContentDisposition cd = new ContentDisposition
            { 
                FileName = attachment.Filename,
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return new FilePathResult("~/Content/uploads/" + attachment.FilenameInternal, MediaTypeNames.Application.Octet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}