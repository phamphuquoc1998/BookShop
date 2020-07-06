using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookVN.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string BookName { get; set; }

        [ForeignKey("FKAuthor")]
        public int AuthorID { get; set; }
        public virtual Author FKAuthor { get; set; }

        [ForeignKey("FKGenre")]
        public int GenreID { get; set; }
        public virtual Genre FKGenre { get; set; }

        [ForeignKey("FKPublisher")]
        public int PublisherID { get; set; }
        public virtual Publisher FKPublisher { get; set; }
        public int Inventory { get; set; }

        public double Price { get; set; }
        public double Price2 { get; set; }
        public double Discount { get; set; }
        public string Content { get; set; }
        public string Image{ get; set; }
        public bool IsActive { get; set; }

    }
}