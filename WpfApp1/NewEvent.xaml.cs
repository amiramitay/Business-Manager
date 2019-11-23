using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for NewEvent.xaml
    /// </summary>
    public partial class NewEvent : Window
    {
        public NewEvent()
        {
            InitializeComponent();
        }

        private void NewEventBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!EventDate.SelectedDate.HasValue || EventTitle.Text.Equals(""))
            {
                    if (!EventDate.SelectedDate.HasValue)
                        MessageBox.Show("Please Select Date");
                    else
                        MessageBox.Show("Please Enter Title");
            }
            else
            {
                if (EventDate.SelectedDate < System.DateTime.Today)
                    MessageBox.Show("You can't create event at the past");
                else
                {
                    Event newEvent = new Event();
                    newEvent.Title = EventTitle.Text;
                    newEvent.When = EventDate.SelectedDate.Value;
                    newEvent.Description = EventDescription.Text;
                    MessageBox.Show("When: " + newEvent.When.ToString() + "\n Title: " + newEvent.Title + "\n Descriptopn: " + newEvent.Description);
                }
            }
        }
    }
}

