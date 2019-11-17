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

            workersInfo=new List<Worker>();
            Worker a = new Worker();
            a.FirstName = "Amir";
            a.LastName = "Amitay";
            a.Phone = "0541111111";
            DateTime date = new DateTime(1993, 3, 30).Date;
          //  MessageBox.Show(date.Date.ToString("MM/dd/yyyy"));
            a.DateOfBirth = date.Date;
            workersInfo.Add(a);
          //  WorkerInfoTable.Columns[1].HeaderStringFormat = "HH:mm:ss";//DefaultCellStyle.Format = "HH:mm:ss";
            WorkerInfoTable.ItemsSource = workersInfo;
       //     WorkerInfoTable.Columns[3].HeaderStringFormat= "HH:mm:ss";
                

        }

  
    }
}


