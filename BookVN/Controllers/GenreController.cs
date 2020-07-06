using BookVN.DatabaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;

namespace BookVN.Controllers
{
    public class GenreController : Controller
    {
        private BookVNContext db = new BookVNContext();
        // GET: Genre
        public ActionResult Index()
        {
            ViewBag.ListGenre = db.TbGenres.ToList();
            return View();
        }

        public ActionResult AddGenre(string GenreName)
        {

            if (GenreName.Length > 0)
            {

                // Tạo một cuốn sách mới
                var Genre = new Genre
                {
                    GenreName = GenreName,
                    IsActive = true
                };

                // Lưu database
                db.TbGenres.Add(Genre);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public ActionResult EditGenre(int id, string GenreName)
        {
            var Genre = db.TbGenres.Find(id);
            Genre.GenreName = GenreName;
            db.SaveChanges();

            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public ActionResult DeleteGenre(int id)
        {
            var Genre = db.TbGenres.Find(id);

            Genre.IsActive = false;

            db.Entry(Genre).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public ActionResult RestoreGenre(int id)
        {
            var Genre = db.TbGenres.Find(id);

            Genre.IsActive = true;

            db.Entry(Genre).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Genre");
        }
    }
}