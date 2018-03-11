using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;
using System.Web.Helpers;
using System.Net;
using System.IO;

namespace LibraryWebsite.Controllers
{
    public class UserController : Controller
    {
        LibraryWebsiteEntities db = new LibraryWebsiteEntities();
        // GET: Login

        public ActionResult Details(int? id)
        {
            ViewBag.header = db.Headers.First();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var btr = db.Books_To_Readers.Where(rd => rd.Reader_id == id).ToList();
            if (btr == null)
            {
                return HttpNotFound();
            }
            return View(btr);          
        }

        public ActionResult Login()
        {
            ViewBag.header = db.Headers.First();

            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            ViewBag.header = db.Headers.First();

            if (Email != "" && Password != "")
            {
                Reader activereader = db.Readers.FirstOrDefault(rd => rd.Email == Email);
                if (activereader != null)
                {
                    if (Crypto.VerifyHashedPassword(activereader.Password, Password))
                    {
                        Session["loggedUser"] = activereader;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.LoginError = "Email or Password is not correct.";
                    }
                }
                else
                {
                    ViewBag.LoginError = "Email or Password is not correct.";
                }
            }
            else
            {
                ViewBag.LoginError = "Please, fill Email and Password.";
            }         
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.header = db.Headers.First();

            return View();
        }

        [HttpPost]
        public ActionResult Register(Reader reader, string RepeatPassword)
        {
            ViewBag.header = db.Headers.First();

            if (reader.Fullname != null && reader.Email != null && reader.Username != null
                && reader.Password != null)
            {
                if(!(db.Readers.Any(rd => rd.Email == reader.Email || rd.Username == reader.Username)))
                {
                    if(reader.Password == RepeatPassword)
                    {
                        reader.Password = Crypto.HashPassword(reader.Password);
                        db.Readers.Add(reader);
                        db.SaveChanges();

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.RegisterError = "Password and repeat password does not match.";
                    }
                }
                else
                {
                    ViewBag.RegisterError = "Username or Email have been used.";
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            ViewBag.header = db.Headers.First();

            Session["loggedUser"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profile(int? id)
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