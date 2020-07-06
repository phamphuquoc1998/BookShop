using BookVN.DatabaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;

namespace BookVN.Controllers
{
    public class UserController : Controller
    {
        private BookVNContext db = new BookVNContext();
        public ActionResult Index()
        {
            ViewBag.ListUser = db.TbUsers.ToList();
            return View();
        }

        // --- GET: Đăng nhập --- //
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }

        // --- POST: Đăng nhập --- //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserName, Password")] BookVN.Models.User user)
        {
            string UserName = user.UserName;
            string Password = user.Password;
            // Kiểm tra Validation
            if (ModelState.IsValid)
            {
                var curentUser = db.TbUsers.Where(m => m.UserName == UserName && m.Password == Password).SingleOrDefault();
                if (curentUser != null)
                {
                    if (!curentUser.IsActive)
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa");
                        return View(user);
                    }
                    else
                    {
                        Session.Add("User", curentUser);

                        //---------------------------------------------------- Giỏ hàng --------------- //
                        // kiểm tra người này đã có giỏ hàng chưa 
                        var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();
                        // Ghi nhận đăng nhập hệ thống
                        Log log = new Log { Actor = curentUser.UserName, Time = DateTime.Now };
                        log.Action = "Đăng Nhập";
                        db.Logs.Add(log);
                        if (userCart == null)
                        {
                            // Nếu chưa có sẽ tạo một giỏ hàng mới
                            Cart cart = new Cart(curentUser.UserID);
                            db.TbCart.Add(cart);
                           
                        }
                        db.SaveChanges();
                        //--------------------------------------------------------- END --------------- //

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(user);
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        // --- GET: Đăng kí --- //
        [HttpGet]
        public ActionResult Register()
        {
            if (Session["User"] != null) return RedirectToAction("Index", "Home");
            return View();
        }
        // --- POST: Đăng kí --- //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserName, Password, Role, IsActive")] BookVN.Models.User user, string pass)
        {
            var found = db.TbUsers.FirstOrDefault(u => u.UserName == user.UserName);
            if (found != null)
            {
                ModelState.AddModelError("", "Người dùng đã tồn tại");
            }
            if (ModelState.IsValid)
            {
                if (user.Password != pass)
                {
                    ModelState.AddModelError("confirmPassword", "Nhập lại mật khẩu không đúng");
                    return View(user);
                }
                db.Entry(user).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Login", "User");
            }
            return View(user);
        }

        // --- POST: Thêm người dùng--- //
        [HttpPost]
        public ActionResult AddUser(string UserName, string Password, string Role)
        {
            db.TbUsers.Add(new Models.User { UserName = UserName, Password = Password, Role = Role });
            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }
        // --- POST: Update người dùng--- //
        [HttpPost]
        public ActionResult EditUser(int id, string UserName, string Password, string Role)
        {
            var User = db.TbUsers.Find(id);
            User.UserName = UserName;
            User.Password = Password;
            User.Role = Role;

            // Log update thông tin người dùng
            Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
            log.Action = $"Cập nhật thông tin người dùng với ID: {User.UserID}";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }
        // --- POST: Xóa người dùng--- //
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            var User = db.TbUsers.Find(id);

            User.IsActive = false;

            db.Entry(User).State = System.Data.Entity.EntityState.Modified;
            Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
            log.Action = $"Xóa người dùng với ID: {User.UserID}";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }
        // --- POST: Khôi phục người dùng--- //
        [HttpPost]
        public ActionResult RestoreUser(int id)
        {
            var User = db.TbUsers.Find(id);

            User.IsActive = true;

            db.Entry(User).State = System.Data.Entity.EntityState.Modified;
            Log log = new Log { Actor = (Session["User"] as User)?.UserName, Time = DateTime.Now };
            log.Action = $"Khôi phục người dùng với ID: {User.UserID}";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }
    }
}