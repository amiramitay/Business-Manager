using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Order
    {
        public Product[] Products { get; set; }
        public Item[] Items { get; set; }
        public int[] Count {get;set;}
        public Customer Customer { get; set; }

    }
}
