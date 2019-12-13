using System;

namespace WpfApp1
{
    public class Worker
    {
        public Worker(string firstName, string lastName, char gender, string phone, DateTime dateOfBirth, string @class, string role, DateTime startWork, bool isUser, int wage)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Gender = gender;
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            DateOfBirth = dateOfBirth;
            Class = @class ?? throw new ArgumentNullException(nameof(@class));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            StartWork = startWork;
            this.isUser = isUser;
            Wage = wage;
        }

        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public char Gender { get; set; }
        public string Phone { get; internal set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Class { get; set; }
        public string Role { get; set; }
        public System.DateTime StartWork { get; set; }
        public bool isUser { get; set; }
        public int Wage { get; set; }


        public int CalcAge()
        {
            var today = System.DateTime.Today;
            int age = (System.Int32.Parse(today.ToString("yyyyMMdd")) - System.Int32.Parse(this.DateOfBirth.ToString("yyyyMMdd"))) / 10000;
            return  age;
        }
        
        public int TimeHeWorks()
        {
            var today = System.DateTime.Today;
            int time = (System.Int32.Parse(today.ToString("yyyyMMdd")) - System.Int32.Parse(this.StartWork.ToString("yyyyMMdd"))) / 10000;
            return time;
        }
        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }

        public enum Classes
        { 
            
 
            Manager,
            Office,
            Sales
            
          
        }
        
       
    }
}