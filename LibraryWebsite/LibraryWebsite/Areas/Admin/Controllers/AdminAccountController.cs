using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;
using System.Web.Helpers;

namespace LibraryWebsite.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        LibraryWebsiteEntities db = new LibraryWebsiteEntities();
        // GET: Admin/AdminAccount
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            if (email != "" && pass != "")
            {
                AdminInfo admin = db.AdminInfoes.Find(1);
                if (admin.email == email && Crypto.VerifyHashedPassword(admin.password, pass))
                {
                    Session["AdminLogged"] = true;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Email or Password is wrong!";
                }
            }
            else
            {
                ViewBag.Error = "Email or Password cannot be empty!";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["AdminLogged"] = null;
            return RedirectToAction("Login", "AdminAccount");            
        }
    }
}