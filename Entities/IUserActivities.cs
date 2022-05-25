using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
     public interface IUserActivities
    {
        User Login(User user);

        bool Logout(User user);

        User Regitration(User user);

        string IssueBook(BookIssue bookIssue);
    }
}
