using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BookVN.Models;

namespace BookVN.DatabaseFile
{
    public class BookVNSeedData : DropCreateDatabaseIfModelChanges<BookVNContext>
    {
        protected override void Seed(BookVNContext context)
        {
            context.TbUsers.Add(new User { UserName = "admin", Password = "admin123", Role = "admin", IsActive = true });
            context.TbUsers.Add(new User { UserName = "guest", Password = "123", Role = "guest", IsActive = true });

            context.TbAuthors.Add(new Author { AuthorName = "Vũ Trọng Phụng", IsActive = true });
            context.TbAuthors.Add(new Author { AuthorName = "Nguyễn Nhật Ánh", IsActive = true });
            context.TbAuthors.Add(new Author { AuthorName = "Thanh Tâm Tài Nhân", IsActive = true });
            context.TbAuthors.Add(new Author { AuthorName = "Lisa Klepas", IsActive = true });
            context.TbAuthors.Add(new Author { AuthorName = "Joseph Murphy", IsActive = true });
            context.TbAuthors.Add(new Author { AuthorName = "Paulo Coelho", IsActive = true });

            context.TbGenres.Add(new Genre { GenreName = "Tiểu thuyết", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Truyện ngắn", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Tập thơ", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Ngoại Văn", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Thiếu nhi", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Giáo khoa - giáo án", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Trinh thám", IsActive = true });
            context.TbGenres.Add(new Genre { GenreName = "Cổ tích", IsActive = true });


            context.TbPublishers.Add(new Publisher { PublisherName = "Phương Nam", IsActive = true });
            context.TbPublishers.Add(new Publisher { PublisherName = "Bình Minh", IsActive = true });
            context.TbPublishers.Add(new Publisher { PublisherName = "NXB Thời Đại", IsActive = true });
            context.TbPublishers.Add(new Publisher { PublisherName = "NXB Tổng hợp TP.HCM", IsActive = true });
            context.TbPublishers.Add(new Publisher { PublisherName = "NXB Dân Trí", IsActive = true });
            base.Seed(context);
        }
    }
}