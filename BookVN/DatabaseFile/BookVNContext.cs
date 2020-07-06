using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BookVN.Models;


namespace BookVN.DatabaseFile
{
    public class BookVNContext : DbContext
    {
        public BookVNContext() : base()
        {
            string databasename = "BookVN";
            this.Database.Connection.ConnectionString = @"Data Source=.;Initial Catalog=" + databasename + ";Trusted_Connection=Yes";

            //this.Database.Connection.ConnectionString = "workstation id=BookVNDA1.mssql.somee.com;packet size=4096;user id=michaelt0520_SQLLogin_2;pwd=ihr83lmb73;data source=BookVNDA1.mssql.somee.com;persist security info=False;initial catalog=BookVNDA1";
        }
        public DbSet<User> TbUsers { get; set; }
        public DbSet<Book> TbBooks { get; set; }

        public DbSet<Author> TbAuthors { get; set; }
        public DbSet<Genre> TbGenres { get; set; }
        public DbSet<Publisher> TbPublishers { get; set; }

        public DbSet<Cart> TbCart { get; set; }
        public DbSet<CartDetail> TbCartDetail { get; set; }

        public DbSet<Comment> TbComment { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}