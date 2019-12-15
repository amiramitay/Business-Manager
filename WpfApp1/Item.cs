using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Item
    {
        public Item(string name, string category, string serialNumber, int count, double cost)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
            Count = count;
            Cost = cost;
        }

        public string Name { get; set; }
        public string Category { get; set; }
        public string SerialNumber { get; set; }
        public int Count { get; set; }
        public Double Cost { get; set; }

    }
}
