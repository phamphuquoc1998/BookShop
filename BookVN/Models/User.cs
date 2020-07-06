using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessage = "Bạn phải nhập tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        public string Password { get; set; }

        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}