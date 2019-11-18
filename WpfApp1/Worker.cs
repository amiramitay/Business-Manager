namespace WpfApp1
{
    public class Worker
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public char MorF { get; set; }
        public string Phone { get; internal set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Class { get; set; }
        public string Role { get; set; }
        public System.DateTime StartWork { get; set; }
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