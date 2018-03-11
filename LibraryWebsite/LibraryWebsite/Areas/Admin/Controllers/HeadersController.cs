using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;
using System.IO;

namespace LibraryWebsite.Areas.Admin.Controllers
{
    [AuthorizationFilterController]
    public class HeadersController : Controller
    {
        private LibraryWebsiteEntities db = new LibraryWebsiteEntities();

        // GET: Admin/Headers
        public ActionResult Index()
        {
            return View(db.Headers.ToList());
        }

        // GET: Admin/Headers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Header header = db.Headers.Find(id);
            if (header == null)
            {
                return HttpNotFound();
            }
            return View(header);
        }

      
       

        // GET: Admin/Headers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Header header = db.Headers.Find(id);
            if (header == null)
            {
                return HttpNotFound();
            }
            return View(header);
        }

        // POST: Admin/Headers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,[Bind(Include = "id,Title,Background")] Header header, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                Header activeHeader = db.Headers.Find(id);
                if (activeHeader != null)
                {
                    string fileName = null;
                    if (Photo != null)
                    {
                        if (Photo.ContentLength > 0 && Photo.ContentLength <= 3 * 1024 * 1024)
                        {
                            if (Photo.ContentType.ToLower() == "image/jpeg" ||
                                Photo.ContentType.ToLower() == "image/jpg" ||
                                Photo.ContentType.ToLower() == "image/png" ||
                                Photo.ContentType.ToLower() == "image/gif"
                            )
                            {
                                var path = Path.Combine(Server.MapPath("~/Uploads/"), activeHeader.Background);

                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }

                                DateTime dt = DateTime.Now;
                                var beforeStr = dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + "_" + dt.Millisecond;
                                fileName = beforeStr + Path.GetFileName(Photo.FileName);
                                var newFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                                Photo.SaveAs(newFilePath);

                                activeHeader.Background = fileName;
                                activeHeader.Title = header.Title;                                
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.EditError = "Photo type is not valid.";
                                return View(activeHeader);
                            }
                        }
                        else
                        {
                            ViewBag.EditError = "Photo type should not be more than 3 MB.";
                            return View(activeHeader);
                        }
                    }
                    else
                    {
                        activeHeader.Title = header.Title;                       
                        db.SaveChanges();
                        return RedirectToAction("Index");
                       
                    }
                }
                else
                {
                    ViewBag.EditError = "Id is not correct.";
                    return View(activeHeader);
                }
            }
            return View(header);
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
