using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Event
    {
        public Event(DateTime when, string title, string description)
        {
            When = when;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public System.DateTime When { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
    }
}
