
namespace ConfigurationService
{
    public class Config
    {
        public string Login { get; set; } = "admin";
        public string Password { get; set; } = "qwerty123";
        public string PercoWebHost { get; set; } = "127.0.0.1";
        public string PathToLog { get; set; } = @"..\Log.txt";
        public int NumberOfDaysEvents { get; set; } = 60;
        public string PathToPercoXML { get; set; } = @"..\perco.xml";
        public string PathToNewEmployeesView { get; set; } = @"D:\scripts_from_percoserver\PERCO.SQL";
    }
}
