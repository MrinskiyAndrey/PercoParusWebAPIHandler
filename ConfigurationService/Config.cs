
namespace ConfigurationService
{
    public static class Config
    {
        public static string Login { get; set; } = "admin";
        public static string Password { get; set; } = "qwerty123";
        public static string PercoWebHost { get; set; } = "127.0.0.1";
        public static string PathToLog { get; set; } = @"..\Log.txt";
        public static int NumberOfDaysEvents { get; set; } = 60;
        public static string PathToPercoXML { get; set; } = @"..\perco.xml";
        public static string PathToNewEmployeesView { get; set; } = @"D:\scripts_from_percoserver\PERCO.SQL";
    }
}
