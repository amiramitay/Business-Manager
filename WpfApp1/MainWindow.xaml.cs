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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
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

            mainTabs.Items.Remove(SalesTab);
            mainTabs.Items.Remove(WorkersInfoTab);
            mainTabs.Items.Remove(WorkersHoursTab);
            mainTabs.Items.Remove(CustomersInfoTab);

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


            UserNameTextBox.Text = "amir";
            PasswordTextBox.Password = "1234";

            Event e = new Event();
            e.When = new DateTime(2019, 11, 23).Date;
            MainCal.SelectedDate = new DateTime(2019, 11, 23).Date;


        }
        public Customer createNewCustomer()
        {
            Customer c = new Customer("amraa", true, "000", "aaa");
            return c;
        }

        //Login Register Control
        private void EnableMenus()
        {
            foreach (MenuItem i in MainMenu.Items)
                i.IsEnabled = true;
            foreach (MenuItem i in SideMenu.Items)
                i.IsEnabled = true;
            MainCal.IsEnabled = true;
        }
        public void Logout()
        {
            if (isLogin)
            {
                if (MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo).ToString().Equals("Yes"))
                {
                    isLogin = false;
                    foreach (MenuItem i in MainMenu.Items)
                    {
                        i.IsEnabled = false;
                        i.IsChecked = false;
                    }

                    foreach (MenuItem i in SideMenu.Items)
                        i.IsEnabled = false;
                    MainCal.IsEnabled = false;
                    mainTabs.Items.Add(LoginTab);
                    LoginTab.IsSelected = true;
                }
                else
                {
                    return;
                }
            }
            else
                return;
           
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text.Equals("amir"))
            {
                if (PasswordTextBox.Password.Equals("1234"))
                {
                    isLogin = true;
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

        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserNameTextBox.Text.Equals("amir"))
                CheckImg.Visibility = Visibility.Visible;
            else 
                CheckImg.Visibility = Visibility.Hidden;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

        }






        //LeftPanel Control
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

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }







        //MainTabs Control
        public void AddAndRemoveTabs(string menuItem)
        {
            switch (menuItem)
            {
                case "CustomersBtn":
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Add(CustomersInfoTab);
                    CustomersInfoTab.IsSelected = true;
                    break;
                case "WorkersBtn":
                    mainTabs.Items.Add(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Add(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    WorkersInfoTab.IsSelected = true;
                    break;
                case "ProviderBtn":
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    //   WorkersInfoTab.IsSelected = true;
                    break;
                case "SalesBtn":
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Add(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    SalesTab.IsSelected = true;
                    break;
                case "SupplyBtn":
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    // WorkersInfoTab.IsSelected = true;
                    break;
                case "OrdersBtn":
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    // WorkersInfoTab.IsSelected = true;
                    break;
            }
        }





        //MainMenu Control
        public void UncheckMenuItems(string menuItem)
        {
            foreach (MenuItem i in MainMenu.Items)
                if (!i.Name.Equals(menuItem))
                    i.IsChecked = false;
        }
        private void CustomersBtn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckMenuItems("CustomersBtn");
            AddAndRemoveTabs("CustomersBtn");
        }
        private void WorkersBtn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckMenuItems("WorkersBtn");
            AddAndRemoveTabs("WorkersBtn");
        }
        private void ProviderBtn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckMenuItems("ProviderBtn");
            AddAndRemoveTabs("ProviderBtn");

        }
        private void SalesBtn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckMenuItems("SalesBtn");
            AddAndRemoveTabs("SalesBtn");

        }
        private void SupplyBtn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckMenuItems("SupplyBtn");
            AddAndRemoveTabs("SupplyBtn");
        }
        private void OrdersBtn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckMenuItems("OrdersBtn");
            AddAndRemoveTabs("OrdersBtn");
        }
        private void CustomersBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainTabs.Items.Remove(CustomersInfoTab);
        }
        private void WorkersBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainTabs.Items.Remove(WorkersInfoTab);
            mainTabs.Items.Remove(WorkersHoursTab);
        }
        private void ProviderBtn_Unchecked(object sender, RoutedEventArgs e)
        {
        }
        private void SalesBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainTabs.Items.Remove(SalesTab);
        }
        private void SupplyBtn_Unchecked(object sender, RoutedEventArgs e)
        {
        }
        private void OrdersBtn_Unchecked(object sender, RoutedEventArgs e)
        {
        }


        //Right Panel Control
        //Calander Control
        private void MainCal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLogin)
                return;

            string str = sender.ToString();


            DateTime selectedDate = DateTime.Parse(str);
            MainCal.SelectedDate = selectedDate;

        }

        private void NewEventBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
                return;
            else
            {
               // Page page = new NewEvent();
              //  Window window = new NewEvent(); 
            }
                
        }


    }
}
