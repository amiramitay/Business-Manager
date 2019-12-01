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
using System.Data;
using System.Configuration;

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
        public bool isEventTab = false;
        public string cn = Properties.Settings.Default.systemDataConnectionString;
        SqlConnection sqlcn = new SqlConnection(Properties.Settings.Default.systemDataConnectionString);
        User user = new User();
       
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Amir");
            mainTabs.Items.Remove(SalesTab);
            mainTabs.Items.Remove(WorkersInfoTab);
            mainTabs.Items.Remove(WorkersHoursTab);
            mainTabs.Items.Remove(CustomersInfoTab);
            mainTabs.Items.Remove(NewEventTab);
            mainTabs.Items.Remove(ProvidersInfoTab);
            mainTabs.Items.Remove(NewProviderTab);
            WorkerInfoTable.RowHeaderWidth = 0; //?????
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


            UserNameTextBox.Text = "ami";
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



        //-----------------------//
        //Login Register Control //
        //-----------------------//
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
                    if (sqlcn.State == ConnectionState.Open)
                        sqlcn.Close();

                    isAdmin = false;
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
        bool userCheck = false;
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

            if (userCheck)
            {
                if (sqlcn.State != ConnectionState.Open)
                    sqlcn.Open();

                if (PasswordTextBox.Password.Equals(loginDT.Rows[userIndex]["Password"].ToString().Trim()))
                {

                    user.UserName = loginDT.Rows[userIndex]["User"].ToString().Trim();
                    user.Password = loginDT.Rows[userIndex]["Password"].ToString().Trim();
                    user.isAdmin = (bool)loginDT.Rows[userIndex]["Admin"];
                    isAdmin = user.isAdmin;
                    sqlcn.Close();
                    UpdateTables();

                    UserNameTextBox.Text = "";
                    PasswordTextBox.Password = "";
                    isLogin = true;
                    EnableMenus();
                    CustomersBtn.IsChecked = true;
                    mainTabs.Items.Remove(LoginTab);
                    loginDT.Clear();
                }
                else
                {
                    PasswordLabel.Visibility = Visibility.Visible;
                }

                sqlcn.Close();
            }
            else
            {
                UserLabel.Visibility = Visibility.Visible;
                PasswordLabel.Visibility = Visibility.Hidden;
            }
        }
        DataTable loginDT = new DataTable();
        int userIndex = 1;
        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserLabel.Visibility = Visibility.Hidden;
            if (sqlcn.State != ConnectionState.Open)
                sqlcn.Open();

            string queryString =
                "SELECT * FROM  users;";
            SqlCommand cmd = new SqlCommand(
                  queryString, sqlcn);

            cmd = sqlcn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryString;
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(loginDT);
            string str;

            CheckImg.Visibility = Visibility.Hidden;

            for (int i = 0; i < loginDT.Rows.Count; i++)
            {

                str = loginDT.Rows[i]["User"].ToString().Trim();
                if (UserNameTextBox.Text.Equals(str))
                {
                    UserLabel.Visibility = Visibility.Hidden;
                    CheckImg.Visibility = Visibility.Visible;
                    userCheck = true;
                    userIndex = i;
                    break;
                }
                else
                {
                    userCheck = false;
                }
            }

            //
            //foreach (DataRow dr in dt.Rows)
            //{
            //    //str = dr["User"].ToString();
            //    str = dr["User"].ToString().Trim();//Replace(" ", string.Empty);

            //    if (UserNameTextBox.Text.Equals(str))
            //    {
            //        CheckImg.Visibility = Visibility.Visible;
            //        userCheck = true;
            //        break;
            //    }
            //    else
            //        userCheck = false;
            //}

            sqlcn.Close();
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordLabel.Visibility = Visibility.Hidden;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

        }



        //-------------------//
        //LeftPanel Control--//
        //-------------------//
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin || isEventTab)
            {
                return;
            }
            else
            {
                // sqlcn.Open();
                // string queryString = "INSERT INTO customers " + "( Name , Phone , Email , Join , VIP)" +
                // "VALUES ('am' , 'am','dd', 2019-1-1 , 'true')";
                // SqlCommand cmd = new SqlCommand(queryString, sqlcn);
                // string queryString = "INSERT INTO users (User,Password,Admin) VALUES (@user,@password,@admin)";
                // SqlCommand cmd = new SqlCommand(queryString, sqlcn);
                // cmd.Parameters.Add("@user", SqlDbType.NVarChar);
                // cmd.Parameters.Add("@password", SqlDbType.NVarChar);
                // cmd.Parameters.Add("@admin", SqlDbType.Bit);
                // cmd.Parameters.Add("@join", SqlDbType.Date);
                // cmd.Parameters.Add("@vip", SqlDbType.Bit);
                // cmd.Parameters.AddWithValue("@user", "dani");
                // cmd.Parameters.AddWithValue("@password", "dani");
                // cmd.Parameters.AddWithValue("@admin", false);
                // set values to parameters from textboxes
                //cmd.Parameters["@user"].Value = "gigi";
                // cmd.Parameters["@password"].Value = "aaaa";
                // cmd.Parameters["@email"].Value = "bbb";
                // cmd.Parameters["@join"].Value = "2017/5/5";
                // cmd.Parameters["@admin"].Value = false;
                // string str = cmd.Parameters["@email"].Value.ToString();
                // cmd.CommandType = CommandType.Text;

                // cmd.ExecuteNonQuery();
                // int row = cmd.ExecuteNonQuery();
                // sqlcn.Close();
                // UpdateCustomersTables();
                // cmd = sqlcn.CreateCommand();
                // cmd.CommandType = CommandType.Text;
                // cmd.CommandText = queryString;
                // DataTable dt = new DataTable();
                // cmd.ExecuteNonQuery();
                // SqlDataAdapter ad = new SqlDataAdapter(cmd);
                // ad.Fill(dt);
                // CustomersInfoTable.ItemsSource = dt.DefaultView;
                // MessageBox.Show("Yes");




                // create sql connection object.  Be sure to put a valid connection string
                // create command object with SQL query and link to connection object
                SqlCommand Cmd = new SqlCommand("INSERT INTO providers " +
                                              "(Name, Phone, Email) " +
                                              "VALUES(@Name, @Phone, @Email)", sqlcn);

                // create your parameters
                //Cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                //Cmd.Parameters.Add("@Phone",SqlDbType.NVarChar);
                //Cmd.Parameters.Add("@Email",SqlDbType.NVarChar);

                Cmd.Parameters.AddWithValue("@Name", "dani");
                Cmd.Parameters.AddWithValue("@Phone", "dani");
                Cmd.Parameters.AddWithValue("@Email", "dani");


                //Cmd.Parameters["@Name"].Value = "fani";
                //Cmd.Parameters["@Phone"].Value = "223";
                //Cmd.Parameters["@Email"].Value = "abc";
            
                sqlcn.Open();

                int RowsAffected = Cmd.ExecuteNonQuery();

                sqlcn.Close();
                UpdateProviderTables();
            }
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin || isEventTab)
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
            if (!isLogin || isEventTab)
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



        //----------------//
        //MainTabs Control//
        //----------------//
        public void AddAndRemoveTabs(string menuItem)
        {
            switch (menuItem)
            {
                case "CustomersBtn":
                    mainTabs.Items.Remove(ProvidersInfoTab);
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Add(CustomersInfoTab);
                    CustomersInfoTab.IsSelected = true;
                    break;

                case "WorkersBtn":
                    mainTabs.Items.Remove(ProvidersInfoTab);
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
                    mainTabs.Items.Add(ProvidersInfoTab);
                    ProvidersInfoTab.IsSelected = true;
                    break;

                case "SalesBtn":
                    mainTabs.Items.Remove(ProvidersInfoTab);
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Add(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    SalesTab.IsSelected = true;
                    break;

                case "SupplyBtn":
                    mainTabs.Items.Remove(ProvidersInfoTab);
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    // WorkersInfoTab.IsSelected = true;
                    break;

                case "OrdersBtn":
                    mainTabs.Items.Remove(ProvidersInfoTab);
                    mainTabs.Items.Remove(WorkersInfoTab);
                    mainTabs.Items.Remove(SalesTab);
                    mainTabs.Items.Remove(WorkersHoursTab);
                    mainTabs.Items.Remove(CustomersInfoTab);
                    // WorkersInfoTab.IsSelected = true;
                    break;
            }
        }




        //----------------//
        //MainMenu Control//
        //----------------//
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
            mainTabs.Items.Remove(ProvidersInfoTab);
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

        //--------------------//
        //Right Panel Control //
        //Calander Control----//
        //--------------------//

        private void MainCal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLogin)
                return;

            string str = sender.ToString();

            DateTime selectedDate = DateTime.Parse(str);
            MainCal.SelectedDate = selectedDate;

        }
        Event newEvent = new Event();
        int selectedIndex = 0;

        //-----------------//
        //New Event Control//
        //-----------------//
        private void NewEventBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogin)
                return;
            else
            {
                foreach (TabItem t in mainTabs.Items)
                    t.IsEnabled = false;
                foreach (MenuItem i in MainMenu.Items)
                    i.IsEnabled = false;

                GroupByBtn.IsEnabled = true;
                SortByBtn.IsEnabled = true;

                selectedIndex = mainTabs.SelectedIndex;
                mainTabs.Items.Clear();
                mainTabs.Items.Add(NewEventTab);
                mainTabs.SelectedItem = NewEventTab;
                isEventTab = true;
                SortByBtn.Visibility = Visibility.Hidden;
                GroupByBtn.Visibility = Visibility.Hidden;

            }

        }

        private void ExitNewEventTab_Click(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem i in MainMenu.Items)
            {
                i.IsEnabled = true;
                if (i.IsChecked)
                {
                    i.IsChecked = false;
                    i.IsChecked = true;
                }
            }

            foreach (TabItem t in mainTabs.Items)
                t.IsEnabled = true;


            mainTabs.Items.Remove(NewEventTab);
            mainTabs.SelectedIndex = selectedIndex;
            GroupByBtn.IsEnabled = true;
            SortByBtn.IsEnabled = true;
            isEventTab = false;
            SortByBtn.Visibility = Visibility.Visible;
            GroupByBtn.Visibility = Visibility.Visible;
        }

        private void AddToCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (!EventDate.SelectedDate.HasValue || EventTitle.Text.Equals(""))
            {
                if (!EventDate.SelectedDate.HasValue && EventTitle.Text.Equals(""))
                {
                    DateValidateLabel.Visibility = Visibility.Visible;
                    TitleValidateLabel.Visibility = Visibility.Visible;
                    DateValidateLabel.Content = "Please select date";
                    TitleValidateLabel.Content = "Please enter title";
                }
                else
                {
                    if (EventTitle.Text.Equals(""))
                    {
                        if (EventDate.SelectedDate < System.DateTime.Today)
                            DateValidateLabel.Visibility = Visibility.Visible;
                        else
                            DateValidateLabel.Visibility = Visibility.Hidden;

                        TitleValidateLabel.Visibility = Visibility.Visible;
                        TitleValidateLabel.Content = "Please enter title";
                    }

                    else
                    {
                        DateValidateLabel.Visibility = Visibility.Visible;
                        DateValidateLabel.Content = "Please select date";
                        TitleValidateLabel.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                TitleValidateLabel.Visibility = Visibility.Hidden;
                if (EventDate.SelectedDate < System.DateTime.Today)
                {
                    DateValidateLabel.Content = "Please select future date";
                    DateValidateLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    DateValidateLabel.Visibility = Visibility.Hidden;
                    Event newEvent = new Event();
                    newEvent.Title = EventTitle.Text;
                    newEvent.When = EventDate.SelectedDate.Value;
                    newEvent.Description = EventDescription.Text;
                    MessageBox.Show("When: " + newEvent.When.ToString() + "\n Title: " + newEvent.Title + "\n Descriptopn: " + newEvent.Description);
                }
            }
        }

        private void EventDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventDate.SelectedDate < DateTime.Today)
                DateValidateLabel.Content = "Please select future date";
            else
                DateValidateLabel.Visibility = Visibility.Hidden;
        }

        //----------//
        //DB Control//
        //----------//
        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }
        public void LoadDB()
        {

            if (sqlcn.State != ConnectionState.Open)
            {
                //   MessageBox.Show("Open");
                sqlcn.Open();
            }
            else
                MessageBox.Show("error");
            DataSet ds = new DataSet();
            string queryString = "SELECT * FROM  users;";
            SqlCommand cmd = new SqlCommand(queryString, sqlcn);

            abc.Items.Clear();
            cmd = sqlcn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryString;
            DataTable dt = new DataTable();
            cmd.ExecuteNonQuery();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            //  ad.Fill(ds);

            foreach (DataRow dr in dt.Rows)
            {
                abc.Items.Add(dr["User"].ToString());
                abc.Items.Add(dr["Password"].ToString());
                abc.Items.Add(dr["Admin"].ToString());
            }
            sqlcn.Close();
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sqlcn.State == ConnectionState.Open)
                sqlcn.Close();
        }
        //--------------//
        //Tables Control//
        //--------------//

        public void UpdateTables()
        {
            UpdateCustomersTables();
            UpdateProviderTables();
        }

        public void UpdateCustomersTables()
        {
            DataTable dt = new DataTable();
            string queryString = "SELECT * FROM  customers;";
            SelectFromDB(queryString, dt);
            CustomersInfoTable.ItemsSource = dt.DefaultView;
        }

        public void UpdateProviderTables()
        {
            DataTable dt = new DataTable();
            string queryString = "SELECT * FROM  providers;";
            SelectFromDB(queryString, dt);
            ProvidersInfoTable.ItemsSource = dt.DefaultView;
        }
        //--------------------//
        //DB Commands Control //
        //--------------------//


        public void SelectFromDB(string queryString, DataTable dt)
        {
            if (sqlcn.State != ConnectionState.Open)
                sqlcn.Open();
            SqlCommand cmd = new SqlCommand(queryString, sqlcn);

            cmd = sqlcn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryString;
            cmd.ExecuteNonQuery();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            sqlcn.Close();
        }


    }

}
