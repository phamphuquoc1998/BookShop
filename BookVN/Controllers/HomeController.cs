using BookVN.DatabaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookVN.Controllers
{
    public class HomeController : Controller
    {
        private BookVNContext db = new BookVNContext();
        public ActionResult Index()
        {
            ViewBag.ListGenre = db.TbGenres.ToList();   
            ViewBag.ListBook = db.TbBooks.ToList();
            return View();
        }
        public ActionResult Discount()
        {
            ViewBag.ListGenre = db.TbGenres.ToList();
            ViewBag.ListBook = db.TbBooks.ToList();
            return View();
        }
        public ActionResult AllBook()
        {
            ViewBag.ListGenre = db.TbGenres.ToList();
            ViewBag.ListBook = db.TbBooks.ToList();
            return View();
        }
    }
}