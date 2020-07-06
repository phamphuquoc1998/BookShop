using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;
using BookVN.DatabaseFile;

namespace BookVN.Controllers
{
    public class CommentController : Controller
    {
        private BookVNContext db = new BookVNContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddComment(int BookID, string Content)
        {
            var curentUser = Session["User"] as User;
            var comment = new Comment
            {
                BookID = BookID,
                UserID = curentUser.UserID,
                Content = Content,
                Time = DateTime.Now
            };

            db.TbComment.Add(comment);
            db.SaveChanges();
            return RedirectToAction("BookDetail", "Book", new { id = BookID });
        }
    }
}