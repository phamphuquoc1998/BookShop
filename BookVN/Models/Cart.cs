using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [ForeignKey("FKUser")]
        public int UserID { get; set; }
        public virtual User FKUser { get; set; }

        public int ProductAmount { get; set; }
        public double TotalMoney { get; set; }

        public Cart() { } // Constructor rỗng 

        public Cart(int UserID)
        {
            this.UserID = UserID;
            ProductAmount = 0;
            TotalMoney = 0;
        }
    }
}