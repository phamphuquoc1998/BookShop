using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [ForeignKey("FKUser")]
        public int UserID { get; set; }
        public virtual User FKUser { get; set; }

        [ForeignKey("FKBook")]
        public int BookID { get; set; }
        public virtual Book FKBook { get; set; }

        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}