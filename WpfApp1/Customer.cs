using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Customer
    {
        public Customer(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }

        public Customer(string name, bool vip, string phone, string email)
        {
            Name = name;
            Vip = vip;
            Phone = phone;
            Email = email;
        }

        public string Name { get; set; }
        public Boolean Vip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public System.DateTime StartWorkWithUs { get; set; }


    }

}
