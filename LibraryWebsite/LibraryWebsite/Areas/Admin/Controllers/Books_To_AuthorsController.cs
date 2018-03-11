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

namespace LibraryWebsite.Areas.Admin.Controllers
{
    [AuthorizationFilterController]
    public class Books_To_AuthorsController : Controller
    {
        private LibraryWebsiteEntities db = new LibraryWebsiteEntities();

        // GET: Admin/Books_To_Authors
        public ActionResult Index()
        {
            var books_To_Authors = db.Books_To_Authors.Include(b => b.Author).Include(b => b.Book);
            return View(books_To_Authors.ToList());
        }

        // GET: Admin/Books_To_Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books_To_Authors books_To_Authors = db.Books_To_Authors.Find(id);
            if (books_To_Authors == null)
            {
                return HttpNotFound();
            }
            return View(books_To_Authors);
        }

        // GET: Admin/Books_To_Authors/Create
        public ActionResult Create()
        {
            ViewBag.Author_id = new SelectList(db.Authors, "id", "Name");
            ViewBag.Book_id = new SelectList(db.Books, "id", "BookName");
            return View();
        }

        // POST: Admin/Books_To_Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Books_To_Authors books_To_Authors, string AuthorName, string Content, HttpPostedFileBase Photo, HttpPostedFileBase PDF)
        {
            
                if (ModelState.IsValid)
                {
                    if (Photo != null && PDF != null)
                    {
               
                    
                        string fileName = null;
                        string pdffileName = null;
                        if (Photo.ContentLength > 0 && Photo.ContentLength <= 3 * 1024 * 1024)
                        {
                            if((Photo.ContentType.ToLower() == "image/jpeg" ||
                                Photo.ContentType.ToLower() == "image/jpg" ||
                                Photo.ContentType.ToLower() == "image/png" ||
                                Photo.ContentType.ToLower() == "image/gif") && 
                                PDF.ContentType.ToLower() == "application/pdf"
                            )
                            {
                                DateTime dt = DateTime.Now;
                                var beforeStr = dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + "_" + dt.Millisecond;
                                fileName = beforeStr + Path.GetFileName(Photo.FileName);
                                pdffileName = beforeStr + Path.GetFileName(PDF.FileName);
                                var newFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                                var newpdfPath = Path.Combine(Server.MapPath("~/Uploads/File/"), pdffileName);

                                var existauthor = db.Authors.FirstOrDefault(au => au.Name == AuthorName);
                                if (existauthor != null)
                                {
                                Photo.SaveAs(newFilePath);
                                PDF.SaveAs(newpdfPath);
                                books_To_Authors.Book.Photo = fileName;
                                books_To_Authors.Book.Content = Content;                                
                                books_To_Authors.Book.PDFfile = pdffileName;
                                books_To_Authors.Author_id = existauthor.id;
                                books_To_Authors.Book.status = 1;
                                db.Books_To_Authors.Add(books_To_Authors);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                                }
                                else
                                {
                                    Author au = new Author();
                                    au.Name = AuthorName;
                                    db.Authors.Add(au);
                                    Photo.SaveAs(newFilePath);
                                    books_To_Authors.Book.Photo = fileName;
                                    books_To_Authors.Book.Content = Content;
                                    books_To_Authors.Book.PDFfile = pdffileName;
                                    books_To_Authors.Author_id = au.id;
                                    books_To_Authors.Book.status = 1;
                                    db.Books_To_Authors.Add(books_To_Authors);
                                    db.SaveChanges();

                                return RedirectToAction("Index");
                                }                   
                            }
                            else
                            {
                                ViewBag.EditError = "Photo type is not valid.";
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.EditError = "Photo type should not be more than 3 MB.";
                            return View();
                        }
                    }
                    ViewBag.EditError = "cover photo and pdf can not be empty.";
                    return View();


                }                                  
                return View(books_To_Authors);
        }

        // GET: Admin/Books_To_Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books_To_Authors books_To_Authors = db.Books_To_Authors.Find(id);
            if (books_To_Authors == null)
            {
                return HttpNotFound();
            }
            ViewBag.Author_id = new SelectList(db.Authors, "id", "Name", books_To_Authors.Author_id);
            ViewBag.Book_id = new SelectList(db.Books, "id", "BookName", books_To_Authors.Book_id);
            return View(books_To_Authors);
        }

        // POST: Admin/Books_To_Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,Books_To_Authors books_To_Authors, HttpPostedFileBase Photo, string Content)
        {
            if (ModelState.IsValid)
            {
                Books_To_Authors selected = db.Books_To_Authors.Find(id);
                if(selected != null)
                {
                    string fileName = null;
                    if(Photo != null)
                    {
                        if (Photo.ContentLength > 0 && Photo.ContentLength <= 3 * 1024 * 1024)
                        {
                            if (Photo.ContentType.ToLower() == "image/jpeg" ||
                                Photo.ContentType.ToLower() == "image/jpg" ||
                                Photo.ContentType.ToLower() == "image/png" ||
                                Photo.ContentType.ToLower() == "image/gif"
                            )
                            {
                                var path = Path.Combine(Server.MapPath("~/Uploads/"), selected.Book.Photo);

                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }

                                DateTime dt = DateTime.Now;
                                var beforeStr = dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + "_" + dt.Millisecond;
                                fileName = beforeStr + Path.GetFileName(Photo.FileName);
                                var newFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                                Photo.SaveAs(newFilePath);

                                selected.Book.Photo = fileName;
                                selected.Book.BookName = books_To_Authors.Book.BookName;
                                selected.Book.Content = Content;
                                selected.Author.Name = books_To_Authors.Author.Name;
                                selected.Book.TotalCount = books_To_Authors.Book.TotalCount;
                                db.SaveChanges();
                                return RedirectToAction("Index");
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
                        selected.Book.BookName = books_To_Authors.Book.BookName;
                        selected.Book.Content = Content;
                        selected.Author.Name = books_To_Authors.Author.Name;
                        selected.Book.TotalCount = books_To_Authors.Book.TotalCount;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.EditError = "Id is not correct.";
                    return View(selected);
                }               
            }
          
            return View(books_To_Authors);
        }

        // GET: Admin/Books_To_Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books_To_Authors books_To_Authors = db.Books_To_Authors.Find(id);
            if (books_To_Authors == null)
            {
                return HttpNotFound();
            }
            return View(books_To_Authors);
        }

        // POST: Admin/Books_To_Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Books_To_Authors books_To_Authors = db.Books_To_Authors.Find(id);
            books_To_Authors.Book.status = 0;          
            db.SaveChanges();
            return RedirectToAction("Index");
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
