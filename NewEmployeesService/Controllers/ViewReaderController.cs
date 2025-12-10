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
       
        public static void RemoveLines(string path, List<string>tabelNumbers)
        {
            try
            {
                string[] allLines = File.ReadAllLines(path);

                //IEnumerable<string> filteredLines = allLines.Where(line => !line.ContainsAny(tabelNumbers, StringComparer.OrdinalIgnoreCase));

                
            }
            catch (Exception ex)
            {
                throw;
            }
        } 
    }
}
