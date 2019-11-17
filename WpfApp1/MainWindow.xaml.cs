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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var workersInfo = new List<Worker>();
            Worker a = new Worker();
            a.FirstName = "Amir";
            a.LastName = "Amitay";
            a.Phone = "0541111111";

            Worker b = new Worker();
            b.FirstName = "Amir";
            b.LastName = "Amitay";
            b.Phone = "0541111111";

            Worker c = new Worker();
            c.FirstName = "Amir";
            c.LastName = "Amitay";
            c.Phone = "0541111111";

        }

    }
}
