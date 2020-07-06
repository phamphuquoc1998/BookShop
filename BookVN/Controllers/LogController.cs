using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookVN.DatabaseFile;
using BookVN.Models;

namespace BookVN.Controllers
{
    public class LogController : Controller
    {
        private BookVNContext db = new BookVNContext();

        // GET: Log
        public ActionResult Index()
        {
            if (Session["User"] == null || (Session["User"] as User).Role != "admin") return RedirectToAction("Login", "User");
            return View(db.Logs.OrderByDescending(log => log.Time).ToList());
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
