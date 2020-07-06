using BookVN.DatabaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;

namespace BookVN.Controllers
{
    public class PublisherController : Controller
    {
        private BookVNContext db = new BookVNContext();
        // GET: Publisher
        public ActionResult Index()
        {
            ViewBag.ListPublisher = db.TbPublishers.ToList();
            return View();
        }

        public ActionResult AddPublisher(string PublisherName)
        {

            if (PublisherName.Length > 0)
            {

                // Tạo một cuốn sách mới
                var Publisher = new Publisher
                {
                    PublisherName = PublisherName,
                    IsActive = true
                };

                // Lưu database
                db.TbPublishers.Add(Publisher);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Publisher");
        }

        [HttpPost]
        public ActionResult EditPublisher(int id, string PublisherName)
        {
            var Publisher = db.TbPublishers.Find(id);
            Publisher.PublisherName = PublisherName;
            db.SaveChanges();

            return RedirectToAction("Index", "Publisher");
        }

        [HttpPost]
        public ActionResult DeletePublisher(int id)
        {
            var Publisher = db.TbPublishers.Find(id);

            Publisher.IsActive = false;

            db.Entry(Publisher).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Publisher");
        }

        [HttpPost]
        public ActionResult RestorePublisher(int id)
        {
            var Publisher = db.TbPublishers.Find(id);

            Publisher.IsActive = true;

            db.Entry(Publisher).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Publisher");
        }
    }
}