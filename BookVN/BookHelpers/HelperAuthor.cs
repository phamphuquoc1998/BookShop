using BookVN.DatabaseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookVN.BookHelpers
{
    public static class HelperAuthor
    {
        private static BookVNContext db = new BookVNContext();
        public static IHtmlString GetAuthorName(int id)
        {
            return new HtmlString(db.TbAuthors.Find(id).AuthorName);
        }
    }
}