using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class BookIssueModel:User
    {
        public List<BookItem> BookItems { get; set; }
   
    }
}