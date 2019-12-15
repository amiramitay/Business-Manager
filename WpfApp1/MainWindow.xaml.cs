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
using System.Text.RegularExpressions;

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
            Window1 window = new Window1();
            window.Show();
            //Worker a = new Worker();
            //a.FirstName = "Amir";
            //a.LastName = "Amitay";
            //a.Phone = "0541111111";
            //a.Role = "Manager";
            //a.Class = "Manager";

            Order o = new Order();



            DateTime date = new DateTime(1993, 3, 30).Date;

            //a.CalcAge();
            //a.DateOfBirth = date.Date;

            //workersInfo.Add(a);
            //WorkerInfoTable.ItemsSource = workersInfo;


            UserNameTextBox.Text = "amir";
            PasswordTextBox.Password = "1234";

            Event e = new Event();
            e.When = new DateTime(2019, 11, 23).Date;
            MainCal.SelectedDate = new DateTime(2019, 11, 23).Date;

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
                TabItem t = ToWhichTable();
                if (t != null)
                {
                    string s = t.Name.ToString();
                    switch (s)
                    {

                        case "ProvidersInfoTab":
                            {

                                string name = "dani";
                                string phone = "054-636-1321";
                                string email = "aaa@bb.com";

                                if ((IsValidPhone(phone) && IsValidEmail(email)) || (IsValidPhone(phone) && email.Equals("")))
                                {
                                    Provider p = createNewProvider(name, email, phone);
                                    AddNewProvider(p);
                                    UpdateProviderTables();
                                }

                                else
                                    if (!IsValidPhone(phone) && !IsValidEmail(email))
                                    MessageBox.Show("Wrong phone and email");
                                else
                                    if (IsValidPhone(phone))
                                    MessageBox.Show("Wrong email");
                                else
                                    MessageBox.Show("Wrong phone");

                                break;
                            }

                        case "WorkersInfoTab":
                            {
                                string fname = "itzik";
                                string lname = "sason";
                                string phone = "0546765432";
                                DateTime join = DateTime.Now;
                                DateTime b = new DateTime(1977, 1, 18);
                                char m = 'm';

                                if (IsValidPhone(phone))
                                {
                                    Worker w = createNewWorker(fname, lname, m, phone, b, "aaa", "bbb", DateTime.Now, false, 10000);
                                    AddNewWorker(w);
                                    UpdateWorkerInfoTables();
                                }
                                else
                                    MessageBox.Show("Wrong phone");

                                break;
                            }

                        case "CustomersInfoTab":
                            {

                                string name = "itzik";
                                string email = "sss@cdsds.com";
                                string phone = "0546765432";
                                DateTime join = DateTime.Now;

                                if (IsValidPhone(phone) && IsValidEmail(email))
                                {
                                    Customer c = createNewCustomer(name, phone, email, join);
                                    AddNewCustomer(c);
                                    UpdateCustomersTables();
                                }

                                else
                                    if (!IsValidPhone(phone) && !IsValidEmail(email))
                                    MessageBox.Show("Wrong phone and email");
                                else
                                    if (IsValidPhone(phone))
                                    MessageBox.Show("Wrong email");
                                else
                                    MessageBox.Show("Wrong phone");

                                break;

                            }

                        case "SalesTab":
                            {
                                break;
                            }


                        default:
                            return;
                    }
                }
                else
                    return;
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
            mainTabs.Items.Add(testTab);
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
                if (EventDate.SelectedDate < DateTime.Today)
                {
                    DateValidateLabel.Content = "Please select future date";
                    DateValidateLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    DateValidateLabel.Visibility = Visibility.Hidden;
                    Event newEvent = CreateNewEvent();
                    newEvent.Title = EventTitle.Text;
                    newEvent.When = EventDate.SelectedDate.Value;
                    newEvent.Description = EventDescription.Text;
                    CreateNewEvent(newEvent);
                    UpdateTestTable();
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


            cmd = sqlcn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = queryString;
            DataTable dt = new DataTable();
            cmd.ExecuteNonQuery();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            //  ad.Fill(ds);

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
            UpdateWorkerInfoTables();
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

        public void UpdateWorkerInfoTables()
        {
            DataTable dt = new DataTable();
            string queryString = "SELECT Id,[First Name],[Last Name],Class,Role FROM  workers;";
            SelectFromDB(queryString, dt);
            WorkerInfoTable.ItemsSource = dt.DefaultView;
        }

        public void UpdateWorkersTables()
        {

        }


        public void UpdateTestTable()
        {
            DataTable dt = new DataTable();
            string queryString = "SELECT *  FROM  events;";
            SelectFromDB(queryString, dt);
            testTable.ItemsSource = dt.DefaultView;
        }


        //--------------------//
        //DB Commands Control //
        //--------------------//


        //insert into tables

        public void AddNewProvider(Provider c)
        {
            SqlCommand Cmd = new SqlCommand("INSERT INTO providers " + "(Name, Phone, Email) " +
                                           "VALUES(@Name, @Phone, @Email)", sqlcn);

            Cmd.Parameters.AddWithValue("@Name", c.Name);
            Cmd.Parameters.AddWithValue("@Phone", c.Phone);
            Cmd.Parameters.AddWithValue("@Email", c.Email);

            sqlcn.Open();

            int RowsAffected = Cmd.ExecuteNonQuery();

            sqlcn.Close();
        }

        public void AddNewCustomer(Customer c)
        {
            SqlCommand Cmd;
            if (c.Email.Equals(""))
            {
                Cmd = new SqlCommand("INSERT INTO customers " + "(Name, Phone, [Join],VIP) " +
                                                        "VALUES(@Name, @Phone, @Join,@VIP)", sqlcn);
                Cmd.Parameters.AddWithValue("@Name", c.Name);
                Cmd.Parameters.AddWithValue("@Phone", c.Phone);
                Cmd.Parameters.AddWithValue("@Join", c.Join.Date);
                Cmd.Parameters.AddWithValue("@VIP", false);
            }
            else
            {
                Cmd = new SqlCommand("INSERT INTO customers" + "(Name, Phone,Email,[Join],VIP) " +
                                            "VALUES(@Name, @Phone,@Email,@Join,@VIP)", sqlcn);
                Cmd.Parameters.AddWithValue("@Name", c.Name);
                Cmd.Parameters.AddWithValue("@Phone", c.Phone);
                Cmd.Parameters.AddWithValue("@Join", c.Join.Date.ToShortDateString());
                Cmd.Parameters.AddWithValue("@VIP", false);
                Cmd.Parameters.AddWithValue("@Email", c.Email);
            }


            sqlcn.Open();

            int RowsAffected = Cmd.ExecuteNonQuery();

            sqlcn.Close();
        }

        public void AddNewWorker(Worker w)
        {
            SqlCommand Cmd;

            Cmd = new SqlCommand("INSERT INTO workers " + "([First Name], [Last Name], Gender, Class, Role, [Date Of Birth], [Start Work], Wage, isUser) " +
                                                    "VALUES(@fname, @lname, @gender,@class,@role,@dob,@SW ,@wage,@user)", sqlcn);

            Cmd.Parameters.AddWithValue("@fname", w.FirstName);
            Cmd.Parameters.AddWithValue("@lname", w.LastName);
            Cmd.Parameters.AddWithValue("@gender", w.Gender);
            Cmd.Parameters.AddWithValue("@class", w.Class);
            Cmd.Parameters.AddWithValue("@role", w.Role);
            Cmd.Parameters.AddWithValue("@dob", w.DateOfBirth);
            Cmd.Parameters.AddWithValue("@SW", w.StartWork);
            Cmd.Parameters.AddWithValue("@wage", w.Wage);
            Cmd.Parameters.AddWithValue("@user", w.isUser);

            sqlcn.Open();

            int RowsAffected = Cmd.ExecuteNonQuery();

            sqlcn.Close();
        }

        public void CreateNewEvent(Event e)
        {
            SqlCommand Cmd;

            Cmd = new SqlCommand("INSERT INTO events " + "(Date, [Title], Description, Username) " +
                                                    "VALUES(@date, @title, @desc,@user)", sqlcn);

            Cmd.Parameters.AddWithValue("@date", e.When.Date.ToShortDateString());
            Cmd.Parameters.AddWithValue("@title", e.Title);
            Cmd.Parameters.AddWithValue("@desc", e.Description);
            Cmd.Parameters.AddWithValue("@user", user.UserName);

            sqlcn.Open();

            int RowsAffected = Cmd.ExecuteNonQuery();

            sqlcn.Close();
        }


        //table selection

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

        public TabItem ToWhichTable()
        {
            MenuItem menuItem = new MenuItem();
            foreach (MenuItem m in MainMenu.Items)
                if (m.IsChecked == true)
                {
                    menuItem = m;
                    break;
                }

            TabItem tabItem = null; ;
            foreach (TabItem t in mainTabs.Items)
            {
                if (t.IsSelected)
                {
                    tabItem = t;
                    break;
                }
            }
            return tabItem;
        }


        //---------//
        //Class Use//
        //---------//

        public Customer createNewCustomer(string name, string phone, string email, DateTime now)
        {
            Customer c = new Customer(name, phone, email, now);
            return c;
        }

        public Provider createNewProvider(string name, string phone, string email)
        {
            Provider p = new Provider(name, phone, email);
            return p;
        }

        public Event CreateNewEvent()
        {
            Event e = new Event();
            return e;
        }

        public Worker createNewWorker(string fName, string lName, char gender, string phone, DateTime birth, string c, string role, DateTime now, bool user, int wage)
        {
            Worker w = new Worker(fName, lName, gender, phone, birth, c, role, now, user, wage);
            return w;
        }


        //-----------------//
        //String Validators//
        //-----------------//

        public static bool IsValidEmail(string email)
        {
            Regex rx = new Regex(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            return rx.IsMatch(email);
        }

        public static bool IsValidPhone(string phone)
        {
            Regex rx = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            return rx.IsMatch(phone);
        }

    }


}
