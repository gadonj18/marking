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

        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachment attachment = db.Attachments.Find(id);
            ContentDisposition cd = new ContentDisposition
            { 
                FileName = attachment.Filename,
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return new FilePathResult("~/App_Data/uploads/" + attachment.FilenameInternal, MediaTypeNames.Application.Octet);
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