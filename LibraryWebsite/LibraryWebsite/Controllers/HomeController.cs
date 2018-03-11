using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;

namespace LibraryWebsite.Controllers
{
    public class HomeController : Controller
    {
        LibraryWebsiteEntities db = new LibraryWebsiteEntities();
        public ActionResult Index()
        {
            ViewBag.header = db.Headers.First();
            return View(db.Books_To_Authors.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books_To_Authors book = db.Books_To_Authors.Find(id);
            if(book == null)
            {
                return HttpNotFound();
            }
            ViewBag.header = db.Headers.First();

            return View(book);

        }

      


    }
}