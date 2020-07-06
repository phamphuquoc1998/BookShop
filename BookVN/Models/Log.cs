using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class Log
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Người thực hiện")]
        public string Actor { get; set; }
        [Display(Name = "Hành động")]
        public string Action { get; set; }
        [Display(Name = "Thời gian")]
        public DateTime? Time { get; set; }
    }
}