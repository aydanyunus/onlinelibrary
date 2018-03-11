using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;

namespace LibraryWebsite.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        LibraryWebsiteEntities db = new LibraryWebsiteEntities();


        [AuthorizationFilterController]
        public ActionResult Index()
        {
            return View();
        }
    }
}