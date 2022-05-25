using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class BookIssue:User
    {
        public List<BookItem> BookItems { get; set; }

    }
}
