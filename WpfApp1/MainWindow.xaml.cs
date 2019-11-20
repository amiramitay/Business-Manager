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
        public List<Worker> workersInfo { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            WorkerInfoTable.RowHeaderWidth = 0;
            workersInfo = new List<Worker>();
            Worker a = new Worker();
            a.FirstName = "Amir";
            a.LastName = "Amitay";
            a.Phone = "0541111111";
            a.Role = "Manager";
            a.Class = "Manager";
            Customer c = createNewCustomer(); 

            DateTime date = new DateTime(1993, 3, 30).Date;
            a.CalcAge();
            a.DateOfBirth = date.Date;
            workersInfo.Add(a);
            WorkerInfoTable.ItemsSource = workersInfo;
            lbllbl.Content = c.Name;
            lbb.Content = a.CalcAge();


    
        }

        public Customer createNewCustomer()
        {
            Customer c = new Customer("amr", true, "000", "aaa");


            return c;
        }

       

        
    }

}


