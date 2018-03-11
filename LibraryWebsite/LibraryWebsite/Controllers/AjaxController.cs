using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWebsite.Models;

namespace LibraryWebsite.Controllers
{
    public class AjaxController : Controller
    {
        LibraryWebsiteEntities db = new LibraryWebsiteEntities();
        // GET: Ajax
        public ActionResult GetBook(int? id)
        {
            object data = null;


            var bk_To_Rd = new Books_To_Readers();
            Reader activeReader = (Reader)Session["loggedUser"];
            var count = db.Books_To_Readers.Where(r => r.Reader_id == activeReader.id);
            
            


            if (count.Count() < 5)
            {
                var total = db.Books.Where(bk => bk.id == id).ToList();
                for (int i = 0; i < total.Count; i++)
                {
                    total[i].TotalCount--;

                }
                bk_To_Rd.Book_id = id;
                bk_To_Rd.Reader_id = activeReader.id;
                db.Books_To_Readers.Add(bk_To_Rd);
                db.SaveChanges();
                var books = db.Books_To_Authors.Where(bk => bk.id == id).Select(bk => new
                {
                    bk.id,
                    bk.Book.BookName,
                    bk.Author.Name
                }).ToList();
                data = new
                {
                    status = 200,
                    message = "success",
                    response = books

                };
                
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
             
                data = new
                {
                    status = 404,
                    message = "error",
                    response = ""

                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Close(int id)
        {
            object data = null;            
            Books_To_Readers booktoreader = db.Books_To_Readers.Find(id);
            db.Books_To_Readers.Remove(booktoreader);
            db.SaveChanges();
         
            var bktr = db.Books_To_Readers.Where(bk => bk.Reader_id == id).Select(bk => new
            {
                bk.id,
                bk.Book.BookName,
            }).ToList();
            data = new
            {
                status = 200,
                message = "success",
                response = bktr

            };
            return Json(data, JsonRequestBehavior.AllowGet);
            
           
        }
    }
}