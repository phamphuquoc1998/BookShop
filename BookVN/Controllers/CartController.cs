using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookVN.Models;
using BookVN.DatabaseFile;
using System.IO;
using ClosedXML.Excel;

namespace BookVN.Controllers
{
    public class CartController : Controller
    {
        private BookVNContext db = new BookVNContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        // Dẫn về giỏ hàng của user hiện tại
        public ActionResult DirectToCartDetail()
        {
            if (Session["User"] != null)
            {
                // Lấy giỏ hàng của ng dùng hiện tại
                var curentUser = Session["User"] as User;
                var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();

                // VD: ~/Cart/CartDetail/id
                return RedirectToAction("CartDetail", "Cart", new { id = userCart.CartID });
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        //--- Lấy Cart và CartDetail ---//
        public ActionResult CartDetail(int id)
        {
            var curentUser = Session["User"] as User;

            // Nếu ng dùng chưa đăng nhập 
            if (curentUser == null)
                return RedirectToAction("Login", "Home");
            else
            {
                // Lấy giỏ hàng của người dùng
                var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();
                // Lấy danh sách chi tiết hàng trong giỏ hàng đó
                var ListCartDetail = db.TbCartDetail.Where(c => c.CartID == userCart.CartID).ToList();
                ViewBag.ListCartDetail = ListCartDetail; // List<CartDetail>

                return View(userCart);
            }
        }

        //--- Thêm CartDetail ---//
        [HttpPost]
        public ActionResult AddItemToCart(int BookID, int Quantity)
        {
            // Lấy giỏ hàng của ng dùng hiện tại
            var curentUser = Session["User"] as User;
            if (curentUser == null) return RedirectToAction("Login", "User");
            var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();
            var item = db.TbCartDetail.Where(m => m.BookID == BookID && m.CartID == userCart.CartID).SingleOrDefault();
            var originBook = db.TbBooks.Find(BookID);
            if (item != null)
            {
                // Nếu người dùng nhập số lượng sách là số nhỏ hơn 0
                if (Quantity <= 0)
                    Quantity = 0;

                // Kiểm tra số lượng tồn, nếu hơn số lượng tồn thì gán số lượng mua = số lượng tồn
                item.Quantity += Quantity;
                if (item.Quantity > originBook.Inventory)
                    item.Quantity = originBook.Inventory;
                item.Total = originBook.Price2 * item.Quantity;
                db.SaveChanges();
                // Tạo CartDetail và lưu vào db
            }
            else
            {
                if (Quantity > originBook.Inventory)
                    Quantity = originBook.Inventory;
                else if (Quantity <= 0)
                    Quantity = 1;

                var cartDetail = new CartDetail
                {
                    BookID = BookID,
                    Quantity = Quantity,
                    CartID = userCart.CartID,
                    Time = DateTime.Now,
                    Total = originBook.Price2 * Quantity,
                };
                db.TbCartDetail.Add(cartDetail);
                db.SaveChanges();
            }

            // Tăng số tiền tổng của Cart lên khi add một sản phẩm mới
            var getAllItem = db.TbCartDetail.Where(m => m.CartID == userCart.CartID).ToList();
            userCart.TotalMoney = getAllItem.Sum(m => m.Total);
            userCart.ProductAmount = getAllItem.Count;

            db.SaveChanges();
            return RedirectToAction("CartDetail", "Cart", new { id = userCart.CartID });
        }

        public ActionResult UpdateQuantity(int id, int ItemQuantity)
        {
            // Lấy giỏ hàng của ng dùng hiện tại
            var curentUser = Session["User"] as User;
            var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();
            var item = db.TbCartDetail.Where(m => m.BookID == id && m.CartID == userCart.CartID).SingleOrDefault();
            var originBook = db.TbBooks.Find(id);

            if (ItemQuantity > 0)
            {
                if (ItemQuantity > originBook.Inventory)
                {
                    item.Quantity = originBook.Inventory;
                    item.Total = originBook.Price2 * item.Quantity;
                    db.SaveChanges();
                }
                else
                {
                    item.Quantity = ItemQuantity;
                    item.Total = originBook.Price2 * item.Quantity;
                    db.SaveChanges();
                }
            }

            // Cập nhật lại tổng số tiền sau khi update số lượng sản phẩm
            var getAllItem = db.TbCartDetail.Where(m => m.CartID == userCart.CartID).ToList();
            userCart.TotalMoney = getAllItem.Sum(m => m.Total);
            db.SaveChanges();

            return RedirectToAction("CartDetail", "Cart", new { id = userCart.CartID });
        }

        public ActionResult RemoveItem(int id)
        {
            // Lấy giỏ hàng của ng dùng hiện tại
            var curentUser = Session["User"] as User;
            var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();
            var item = db.TbCartDetail.Where(m => m.BookID == id && m.CartID == userCart.CartID).SingleOrDefault();
            if (item != null)
            {
                db.TbCartDetail.Remove(item);
                userCart.TotalMoney -= item.Total;
                db.SaveChanges();
            }

            userCart.ProductAmount = db.TbCartDetail.Where(m => m.CartID == userCart.CartID).ToList().Count;
            db.Entry(userCart).State = System.Data.Entity.EntityState.Modified;

            return RedirectToAction("CartDetail", "Cart", new { id = userCart.CartID });
        }

        public ActionResult Pay()
        {
            // Lấy giỏ hàng của ng dùng hiện tại
            var curentUser = Session["User"] as User;
            var userCart = db.TbCart.Where(c => c.UserID == curentUser.UserID).SingleOrDefault();

            // Lấy danh sách chi tiết hàng trong giỏ hàng đó
            var ListCartDetail = db.TbCartDetail.Where(c => c.CartID == userCart.CartID).ToList();
            if (ListCartDetail.Count != 0)
            {
                using (var wb = new XLWorkbook())
                {
                    var worksheet = wb.Worksheets.Add("Hoa Don Sach");
                    worksheet.Cell("A1").Value = "Cty TNHH Sách BookVN";
                    worksheet.Cell("A2").Value = "155 Sư Vạn Hạnh (nối dài), phường 13, Quận 10, Thành phố Hồ Chí Minh, Việt Nam";
                    worksheet.Cell("A3").Value = "Điện thoại: (+84 28) 38 632 052 - 38 629 232";
                    worksheet.Cell("A4").Value = "Email: contact@huflit.edu.vn";

                    worksheet.Cell("B6").Value = "PHIẾU MUA HÀNG";

                    worksheet.Cell("A8").Value = "MÃ SÁCH";
                    worksheet.Cell("B8").Value = "TÊN SÁCH";
                    worksheet.Cell("C8").Value = "NGÀY ĐẶT";
                    worksheet.Cell("D8").Value = "SL";
                    worksheet.Cell("E8").Value = "GIÁ";

                    int row = 9;
                    double price = 0;
                    foreach (var data in ListCartDetail)
                    {
                        price += data.Total;
                        worksheet.Cell("A" + row.ToString()).Value = data.BookID;
                        worksheet.Cell("B" + row.ToString()).Value = data.FKBook.BookName;
                        worksheet.Cell("C" + row.ToString()).Value = data.Time;
                        worksheet.Cell("D" + row.ToString()).Value = data.Quantity;
                        worksheet.Cell("E" + row.ToString()).Value = data.Total;

                        row++;

                        data.FKBook.Inventory -= data.Quantity;
                    }

                    worksheet.Cell("A" + row.ToString()).Value = "TỔNG TIỀN";
                    worksheet.Range("A" + row.ToString() + ":D" + row.ToString()).Merge();

                    worksheet.Cell("E" + row.ToString()).Value = price;

                    var rngData = worksheet.Range("A8:E" + row.ToString());
                    var excelTable = rngData.CreateTable();

                    row += 2;

                    worksheet.Cell("A" + row.ToString()).Value = "Số tiền cần thanh toán";
                    worksheet.Range("A" + row.ToString() + ":B" + row.ToString()).Merge();
                    worksheet.Cell("C" + row.ToString()).Value = price;
                    worksheet.Cell("D" + row.ToString()).Value = "vnđ";
                    row++;

                    worksheet.Cell("A" + row.ToString()).Value = "Ngày thanh toán";
                    worksheet.Range("A" + row.ToString() + ":B" + row.ToString()).Merge();
                    worksheet.Cell("C" + row.ToString()).Value = DateTime.Now;

                    MemoryStream stream = GetStream(wb);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=HoaDonSach.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }

                db.TbCartDetail.RemoveRange(ListCartDetail);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }
}