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
        public bool isLogin = false;
        public bool isAdmin = false;

        public MainWindow()
        {

            InitializeComponent();
            foreach (TabItem t in mainTabs.Items)
            {
                if (!t.Name.Equals("LoginTab"))
                    t.Visibility = Visibility.Hidden;
            }




            WorkerInfoTable.RowHeaderWidth = 0;
            workersInfo = new List<Worker>();
            Worker a = new Worker();
            a.FirstName = "Amir";
            a.LastName = "Amitay";
            a.Phone = "0541111111";
            a.Role = "Manager";
            a.Class = "Manager";

            Order o = new Order();
           
            Customer c = createNewCustomer();
            o.Customer = c;
            DateTime date = new DateTime(1993, 3, 30).Date;
            a.CalcAge();
            a.DateOfBirth = date.Date;
            workersInfo.Add(a);
            WorkerInfoTable.ItemsSource = workersInfo;
            lbllbl.Content = o.Customer.Name;
            lbb.Content = a.CalcAge();



        }

        public Customer createNewCustomer()
        {
            Customer c = new Customer("amraa", true, "000", "aaa");


            return c;
        }

        private void EnableMenus()
        {
            foreach (MenuItem i in MainMenu.Items)
                i.IsEnabled = true;


            foreach (MenuItem i in SideMenu.Items)
                i.IsEnabled = true;

            MainCal.IsEnabled = true;

        }

        public void Disconnect()
        {

            if (MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo).ToString().Equals("Yes"))
            {
                isLogin = false;
                foreach (MenuItem i in MainMenu.Items)
                    i.IsEnabled = false;


                foreach (MenuItem i in SideMenu.Items)
                    i.IsEnabled = false;

                MainCal.IsEnabled = false;
            }
            else
            {
                lbb.Content = "no";

            }

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text.Equals("amir"))
            {
                if (PasswordTextBox.Password.Equals("1234"))
                {

                    isLogin = true;
                    MessageBox.Show("Login");
                    EnableMenus();
                    CustomersBtn.IsChecked = true;
                    mainTabs.Items.Remove(LoginTab);

                }

                else
                    MessageBox.Show("Not login");

            }

            else
                MessageBox.Show("Not login");
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
            {
                return;
            }
            else
            {
                MessageBox.Show("Yes");

            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
            {
                return;
            }
            else
            {
                MessageBox.Show("Yes");

            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
            {
                return;
            }
            else
            {
                MessageBox.Show("Yes");

            }
        }

        private void AddBtn_MouseMove(object sender, MouseEventArgs e)
        {

        }


        public void AddAndRemoveTabs(string menuItem)
        {
            switch (menuItem)
            {
                case "CustomersBtn":
                    mainTabs.Items.Add(CustomersInfoTab);
                    break;
                case "WorkersBtn":
                    break;
                case "ProviderBtn":
                    break;
                case "SalesBtn":
                    break;


            }
        }

        public void UncheckMenuItems(string menuItem)
        {
            foreach (MenuItem i in MainMenu.Items)
                if (!i.Name.Equals(menuItem))
                    i.IsChecked = false;


        }

        private void CustomersBtn_Checked(object sender, RoutedEventArgs e)
        {
            AddAndRemoveTabs("CustomersBtn");
            UncheckMenuItems("CustomersBtn");
        }

        private void WorkersBtn_Checked(object sender, RoutedEventArgs e)
        {
            AddAndRemoveTabs("WorkersBtn");
            UncheckMenuItems("WorkersBtn");
        }

        private void ProviderBtn_Checked(object sender, RoutedEventArgs e)
        {
            AddAndRemoveTabs("ProviderBtn");
            UncheckMenuItems("ProviderBtn");

        }

        private void SalesBtn_Checked(object sender, RoutedEventArgs e)
        {
            AddAndRemoveTabs("SalesBtn");
            UncheckMenuItems("SalesBtn");
        }

        private void SupplyBtn_Checked(object sender, RoutedEventArgs e)
        {
            AddAndRemoveTabs("SupplyBtn");
            UncheckMenuItems("SupplyBtn");
        }

        private void OrdersBtn_Checked(object sender, RoutedEventArgs e)
        {
            AddAndRemoveTabs("OrdersBtn");
            UncheckMenuItems("OrdersBtn");
        }
    }
}




