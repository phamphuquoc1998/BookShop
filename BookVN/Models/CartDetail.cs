using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class CartDetail
    {
        [Key]
        public int CartDetailID { get; set; }

        [ForeignKey("FKCart")]
        public int CartID { get; set; }
        public virtual Cart FKCart { get; set; }

        [ForeignKey("FKBook")]
        public int BookID { get; set; }
        public virtual Book FKBook { get; set; }
        public int Quantity { get;set; }
        public double Total { get; set; }
        public DateTime Time { get; set; }
    }
}