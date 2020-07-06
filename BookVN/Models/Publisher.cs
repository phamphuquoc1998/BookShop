using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
        public bool IsActive { get; set; }
    }
}