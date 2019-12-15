using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class User
    {
        public User(string userName, string password, bool isAdmin)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            this.isAdmin = isAdmin;
        }

        public User()
        {

        }

        public String UserName { get; set; }
        public String Password { get; set; }
        public bool isAdmin { get; set; }


    }
}
