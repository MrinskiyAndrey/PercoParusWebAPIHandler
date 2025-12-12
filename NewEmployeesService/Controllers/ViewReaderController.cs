using ConfigurationService;
using NewEmployeesService.Models;
using NewEmployeesService.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NewEmployeesService.Controllers
{
    public static class ViewReaderController
    {
        /// <summary>
        /// Метод принимает путь к текстовому файлу, считывает текст и возвращает его содержимое
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<string> ReadViewAsync(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return await File.ReadAllTextAsync(path, Encoding.GetEncoding(1251));
            }
            catch (Exception ex)
            {
                Logger.Log($"ViewReaderController.ReadView: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Метод расширения который проверяет, содержит ли строка любой из указанных слов с учетом типа сравнения
        /// </summary>
        /// <param name="source"></param>
        /// <param name="words"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        private static bool ContainsAny(this string source, IEnumerable<string> words, StringComparison comparisonType)
        {
            
            return words.Any(word => source.Contains(word, comparisonType));
        }


        /// <summary>
        /// Метод удаляет строки из файла, содержащие указанные табельные номера
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tabelNumbers"></param>
        public static void RemoveLines(string path, List<string>tabelNumbers)
        {
            try
            {
                string[] allLines = File.ReadAllLines(path);

                IEnumerable<string> filteredLines = allLines.Where(line => !line.ContainsAny(tabelNumbers, StringComparison.OrdinalIgnoreCase));

                File.WriteAllLines(path, filteredLines);
            }
            catch (Exception ex)
            {
                Logger.Log($"ViewReaderController.RemoveLines: {ex.Message}");
               
            }
        } 




    }
}
