using BookVN.DatabaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;

namespace BookVN.Controllers
{
    public class AuthorController : Controller
    {
        private BookVNContext db = new BookVNContext();
        // GET: Author
        public ActionResult Index()
        {
            ViewBag.ListAuthor = db.TbAuthors.ToList();
            return View();
        }

        public ActionResult AddAuthor(string AuthorName)
        {

            if (AuthorName.Length > 0)
            {

                // Tạo một cuốn sách mới
                var author = new Author
                {
                    AuthorName = AuthorName,
                    IsActive = true
                };

                // Lưu database
                db.TbAuthors.Add(author);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Author");
        }

        [HttpPost]
        public ActionResult EditAuthor(int id, string AuthorName)
        {
            var author = db.TbAuthors.Find(id);
            author.AuthorName = AuthorName;
            db.SaveChanges();

            return RedirectToAction("Index", "Author");
        }

        [HttpPost]
        public ActionResult DeleteAuthor(int id)
        {
            var author = db.TbAuthors.Find(id);

            author.IsActive = false;

            db.Entry(author).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Author");
        }

        [HttpPost]
        public ActionResult RestoreAuthor(int id)
        {
            var author = db.TbAuthors.Find(id);

            author.IsActive = true;

            db.Entry(author).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Author");
        }
    }
}