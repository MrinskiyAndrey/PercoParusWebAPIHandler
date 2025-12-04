
namespace ConfigurationService
{
    public static class Logger
    {
        //Путь к файлу логов
        private static string _pathToFileLog = @"..\Log.txt";

        // Статическове событие к которому можно подписаться
        private static event Action<string> _eventLog;

        // метод для вызова событий логирования
        public static void Log(string message)
        {
            _eventLog?.Invoke($"[EVENT] {DateTime.Now} : {message}{Environment.NewLine}");
        }

        // Метод обработчик события (Запись в файл)
        private static void WriteToFile(string message)
        {
            try
            {
                File.WriteAllText(_pathToFileLog, message);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ошибка записи в Log файл {ex.Message}");
            }
        }

        // Статический конструктор для инициализации событий логирования по умолчанию
        static Logger()
        {
            _eventLog += WriteToFile;
            _eventLog += Console.WriteLine;
        }
    }
}
