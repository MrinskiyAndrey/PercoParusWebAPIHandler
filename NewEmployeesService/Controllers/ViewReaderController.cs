using ConfigurationService;
using NewEmployeesService.Models;
using NewEmployeesService.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewEmployeesService.Controllers
{
    public static class ViewReaderController
    {
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


        public static List<EmployeeData> GetNewEmployee(string viewContent)
        {
            List<EmployeeData> newEmployees = new List<EmployeeData>();

            try
            {
                // Очистка данных от вызова процедур Perco-S20 и их синтраксических лексем
                string clearedContent = viewContent
                    .Replace("\r", string.Empty)
                    .Replace("execute procedure CONVERT_CARD(", string.Empty)
                    .Replace(");", string.Empty)
                    .Replace("'", string.Empty);

                // Разделение данных на массив строк
                string[] lines = clearedContent.Split('\n');

                // Удаление повторяющихся записей
                string[]? uniqueLines = lines?.Distinct().ToArray();

                // Преобразование строк в объекты Employee
                if (uniqueLines != null)
                {
                    foreach (string? line in uniqueLines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            List<string> lineData = line.Split(',').ToList();

                            newEmployees.Add(new EmployeeData
                            {
                                Position = int.TryParse( lineData[0],out int position) ? position : null,
                                Division = int.TryParse(lineData[1], out int divison) ? divison : null,
                                TabelNumber = lineData[2],
                                FirstName = lineData[3],
                                LastName = lineData[4],
                                MiddleName = lineData[5],

                            });
                        }
                    }
                }
                
                return newEmployees;
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка очистки данных в ViewReaderController " + ex.Message);
                return new List<EmployeeData>();
            }

        }
    }
}
