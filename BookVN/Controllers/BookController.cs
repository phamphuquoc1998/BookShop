using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;
using BookVN.DatabaseFile;
using System.IO;
using System.Globalization;

namespace BookVN.Controllers
{
    public class BookController : Controller
    {
        private BookVNContext db = new BookVNContext();
        // GET: Book
        public ActionResult Index()
        {
            // Danh sách tác giả, thể loại, NXB
            ViewBag.ListAuthor = db.TbAuthors.ToList();
            ViewBag.ListGenre = db.TbGenres.ToList();
            ViewBag.ListPublish = db.TbPublishers.ToList();

            // Danh sách sách
            ViewBag.ListBook = db.TbBooks.ToList();
            return View();
        }
        public ActionResult AddBook(string BookName, int aID, int gID, int bID, double Price, double Discount, int Inventory, string Content, HttpPostedFileBase Image)
        {
            // Lấy ảnh bình cho sách
            string path = "";
            if (Image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(Image.FileName);
                path = Path.Combine(Server.MapPath("~/BookImage"), fileName);
                Image.SaveAs(path);

                // Tạo một cuốn sách mới
                var book = new Book
                {
                    BookName = BookName,
                    AuthorID = aID,
                    GenreID = gID,
                    PublisherID = bID,
                    Price = Price,
                    Price2 = Price - Price * (Discount / 100),
                    Discount = Discount,
                    Inventory = Inventory,
                    Content = Content,
                    Image = fileName,
                    IsActive = true
                };

                // Lưu database
                db.TbBooks.Add(book);
                Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
                log.Action = $"Thêm sách với tên: {book.BookName}";
                db.Logs.Add(log);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public ActionResult EditBook(int id, string BookName, int aID, int gID, int bID, double Price, double Discount, int Inventory, HttpPostedFileBase Image)
        {
            // Lấy ảnh bình cho sách
            string path = "";
            if (Image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(Image.FileName);
                path = Path.Combine(Server.MapPath("~/BookImage"), fileName);
                Image.SaveAs(path);

                var book = db.TbBooks.Find(id);
                book.BookName = BookName;
                book.AuthorID = aID;
                book.GenreID = gID;
                book.PublisherID = bID;
                book.Price = Price;
                book.Price2 = Price - Price * (Discount / 100);
                book.Discount = Discount;
                book.Inventory = Inventory;
                book.Image = fileName;

                Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
                log.Action = $"Sửa sách với tên: {book.BookName}";
                db.Logs.Add(log);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public ActionResult DeleteBook(int id)
        {
            var book = db.TbBooks.Find(id);

            book.IsActive = false;

            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
            log.Action = $"Xóa sách với tên: {book.BookName}";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public ActionResult RestoreBook(int id)
        {
            var book = db.TbBooks.Find(id);

            book.IsActive = true;

            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
            log.Action = $"Khơi phục sách với tên: {book.BookName}";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index", "Book");
        }

        //--- Chi tiết sách + Danh sách Comment ---//
        public ActionResult BookDetail(int id)
        {
            // Tìm sách qua id
            var book = db.TbBooks.Find(id);
            // Tất cả comment qua id sách
            ViewBag.ListComment = db.TbComment.Where(c => c.BookID == id).ToList();

            //if (book != null)
            //{
            return View(book);
            //}
            //else
            //    return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SearchBook(string value)
        {
            if (value != "")
            {
                if (value.Contains(">") && !value.Contains("="))
                {
                    try
                    {
                        string priceValue = value.Replace(">", "");
                        double price = Convert.ToDouble(priceValue);
                        var result = db.TbBooks.Where(m => m.Price > price);

                        ViewBag.ListBook = result.ToList();
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                    catch
                    {
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                }
                else if (value.Contains(">="))
                {
                    try
                    {
                        string priceValue = value.Replace(">=", "");
                        double price = Convert.ToDouble(priceValue);
                        var result = db.TbBooks.Where(m => m.Price >= price);

                        ViewBag.ListBook = result.ToList();
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                    catch
                    {
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                }
                else if (value.Contains("<") && !value.Contains("="))
                {
                    try
                    {
                        string priceValue = value.Replace("<", "");
                        double price = Convert.ToDouble(priceValue);
                        var result = db.TbBooks.Where(m => m.Price < price);

                        ViewBag.ListBook = result.ToList();
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                    catch
                    {
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                }
                else if (value.Contains("<="))
                {
                    try
                    {
                        string priceValue = value.Replace("<=", "");
                        double price = Convert.ToDouble(priceValue);
                        var result = db.TbBooks.Where(m => m.Price <= price);

                        ViewBag.ListBook = result.ToList();
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                    catch
                    {
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                }
                else if(value.Contains("=") && !value.Contains("<") && !value.Contains(">"))
                {
                    try
                    {
                        string priceValue = value.Replace("=", "");
                        double price = Convert.ToDouble(priceValue);
                        var result = db.TbBooks.Where(m => m.Price == price);

                        ViewBag.ListBook = result.ToList();
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                    catch
                    {
                        ViewBag.ListGenre = db.TbGenres.ToList();
                        ViewBag.Value = value;
                        return View();
                    }
                }
                else
                {
                    var result = db.TbBooks.Where(m => m.BookName.Contains(value) || m.FKAuthor.AuthorName.Contains(value) || m.FKGenre.GenreName.Contains(value) || m.FKPublisher.PublisherName.Contains(value) || m.Content.Contains(value) || m.Discount.ToString().Contains(value));

                    ViewBag.ListBook = result.ToList();
                    ViewBag.ListGenre = db.TbGenres.ToList();
                    ViewBag.Value = value;
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Genre(int id)
        {
            ViewBag.Genre = db.TbGenres.Find(id);
            ViewBag.ListGenre = db.TbGenres.ToList();
            ViewBag.ListBookOfGenre = db.TbBooks.Where(m => m.GenreID == id).ToList();
            return View();
        }
        public ActionResult Author(int id)
        {
            ViewBag.Author = db.TbAuthors.Find(id);
            ViewBag.ListAuthor = db.TbAuthors.ToList();
            ViewBag.ListBookOfAuthor = db.TbBooks.Where(m => m.AuthorID == id).ToList();
            return View();
        }
        public ActionResult Publisher(int id)
        {
            ViewBag.Publisher = db.TbPublishers.Find(id);
            ViewBag.ListPublisher = db.TbPublishers.ToList();
            ViewBag.ListBookOfPublisher = db.TbBooks.Where(m => m.PublisherID == id).ToList();
            return View();
        }
    }
}