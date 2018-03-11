using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;

namespace LibraryWebsite.Controllers
{
    public class ReadersController : Controller
    {
        LibraryWebsiteEntities db = new LibraryWebsiteEntities();
        // GET: Readers
        public ActionResult Index()
        {
            ViewBag.header = db.Headers.First();
            return View(db.Readers.ToList());
        }

        public ActionResult Details(int? id)
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
    }
}