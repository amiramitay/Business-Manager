﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Provider
    {
        public Provider(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
