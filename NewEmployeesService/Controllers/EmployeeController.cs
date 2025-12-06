using ConfigurationService;
using NewEmployeesService.Models;
using NewEmployeesService.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NewEmployeesService.Controllers
{
    public static class EmployeeController
    {
        /// <summary>
        /// Метод принимает необработанный текст из View извлекает данные и возвращает List объектов Employee
        /// 
        /// </summary>
        /// <param name="viewRawContent"></param>
        /// <returns></returns>
        public static List<Employee>? ExtractingDataFromParus(string viewRawContent)
        {
            if (!string.IsNullOrEmpty(viewRawContent))
            {
                List<Employee>? viewContent = new List<Employee>();

                string pattern = @"\'(.*?)\'";

                try
                {
                    // Разделение данных на массив строк
                    var lines = viewRawContent.Replace("\r", string.Empty).Split('\n');

                    // Удаление повторяющихся записей
                    var uniqueLines = lines.Distinct().ToList();
                    List<string> employeeContent = new List<string>();
                    foreach (string line in uniqueLines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            var matches = Regex.Matches(line, pattern);

                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                {
                                    employeeContent.Add(match.Groups[1].Value);
                                }
                            }

                            viewContent.Add(new Employee
                            {
                                Position = employeeContent[0],
                                Division = employeeContent[1],
                                TabelNumber = employeeContent[2],
                                FirstName = employeeContent[3],
                                LastName = employeeContent[4],
                                MiddleName = employeeContent[5],
                                
                            });
                            employeeContent.Clear();
                        }
                    }

                    return viewContent;
                }
                catch (Exception ex)
                {

                    Logger.Log($"Ошибка получения данных в EmployeeController.ExtractingDataFromParus:  {ex.Message}");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static List<EmployeeData>? CreateEmployeeData(List<Employee> employees, List<PositionData> positionsFromPerco, List<DivisionData>divisionsFromPerco)
        {
            if (employees.Count > 0 && positionsFromPerco != null)
            {
                var newEmployeesData = new List<EmployeeData>();

                if (employees.Count > 0)
                {
                    foreach (var employee in employees)
                    {
                        int positionId = PositionController.GetPositionIdByName(employee.Position, positionsFromPerco);
                        int divisionId = DivisionController.GetDivisionIdByName(employee.Division, divisionsFromPerco);
                        newEmployeesData.Add(new EmployeeData
                        {
                            Position = positionId,
                            Division = divisionId,
                            TabelNumber = employee.TabelNumber,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            MiddleName = employee.MiddleName,
                            HiringDate = DateTime.Today.ToString("yyyy-MM-dd")

                        });
                    }
                }

                return newEmployeesData;
            }
            else return null;
        }


        public static async Task AddEmployee(HttpClient client, string token, List<EmployeeData> newEmployees)
        {
            var url = $"users/staff?token={token}";

            if (newEmployees.Count > 0)
            {
                foreach (var employee in newEmployees)
                {
                    var employeeJson = JsonSerializer.Serialize(employee);
                    var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    var id = JsonSerializer.Deserialize<IdData>(responseBody);
                    
                }
            }
        }


    }
}
