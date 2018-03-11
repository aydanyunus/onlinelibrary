using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;

namespace LibraryWebsite.Controllers
{
    public class ReadersEditController : Controller
    {
        private LibraryWebsiteEntities db = new LibraryWebsiteEntities();       

        public ActionResult Edit(int? id)
        {
            ViewBag.header = db.Headers.First();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reader reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Reader reader, string Content, HttpPostedFileBase pic)
        {
            ViewBag.header = db.Headers.First();
            if (ModelState.IsValid)
            {
                Reader selected = db.Readers.Find(id);
                if (selected != null)
                {
                    string fileName = null;
                    if (pic != null)
                    {
                        if (pic.ContentLength > 0 && pic.ContentLength <= 3 * 1024 * 1024)
                        {
                            if (pic.ContentType.ToLower() == "image/jpeg" ||
                                pic.ContentType.ToLower() == "image/jpg" ||
                                pic.ContentType.ToLower() == "image/png" ||
                                pic.ContentType.ToLower() == "image/gif"
                            )
                            {
                                                             

                                DateTime dt = DateTime.Now;
                                var beforeStr = dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + "_" + dt.Millisecond;
                                fileName = beforeStr + Path.GetFileName(pic.FileName);
                                var newFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                                pic.SaveAs(newFilePath);

                                selected.Photo = fileName;
                                selected.Content = Content;
                                db.SaveChanges();
                                return RedirectToAction("Edit", "ReadersEdit");
                            }
                            else
                            {
                                ViewBag.EditError = "Photo type is not valid.";
                                return View(selected);
                            }
                        }
                        else
                        {
                            ViewBag.EditError = "Photo type should not be more than 3 MB.";
                            return View(selected);
                        }
                    }
                    else
                    {
                        selected.Content = Content;
                        db.SaveChanges();
                        return RedirectToAction("Edit");
                    }
                }
                else
                {
                    ViewBag.EditError = "Id is not correct.";
                    return View(selected);
                }
            }
            return View(reader);
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
