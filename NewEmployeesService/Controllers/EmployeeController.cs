using ConfigurationService;
using NewEmployeesService.Models;
using NewEmployeesService.Models.DTO;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

                    // Извлечение данных по сотруддникам 
                    List<string> employeeContent = new List<string>();
                    foreach (string line in lines)
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
                            // Lобавление в список класса Employee
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

                    // Отсеивание старых записей
                    var employeeList = viewContent
                        .GroupBy(emp =>emp.TabelNumber)
                        .Select(group => group.Last())
                        .ToList();

                    return employeeList;
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


        public static async Task EditEmployee(HttpClient client, string token, EmployeeData employee)
        {
            var url = $"users/staff/{employee.Id}?token={token}";
            try
            {
                var employeeJson = JsonSerializer.Serialize(employee);
                var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Редактирование сотрудника: " + responseBody + $"Content: \n{await content.ReadAsStringAsync()}");

            }
            catch (Exception ex)
            {
                Logger.Log($"Ошибка изменения сотрудника в PercoWeb в NewEmployeesService.Controllers.EmployeeController.RechangeEmployee: {ex.Message}");
            }
        }


        public static async Task AddEmployee(HttpClient client, string token, List<EmployeeData> newEmployees)
        {
            var url = $"users/staff?token={token}";

            var alreadyExistEmployees = new List<EmployeeData>();

            if (newEmployees.Count > 0)
            {
                try
                {
                    foreach (var employee in newEmployees)
                    {
                        var employeeJson = JsonSerializer.Serialize(employee);
                        var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");
                        var response = await client.PutAsync(url, content);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        if(responseBody.Contains("id"))
                        {
                            employee.Id = JsonSerializer.Deserialize<IdData>(responseBody)?.Id;
                            Logger.Log($"Успешно добавлен новый сотрудник: '{employee.FirstName}' '{employee.LastName}' '{employee.MiddleName}',  табельный номер '{employee.TabelNumber}' в PercoWeb, id '{employee.Id}',");

                        }
                        else if (responseBody.Contains("Такой табельный номер уже существует"))
                        {
                            alreadyExistEmployees.Add(employee);
                        }
                        else
                        {
                            Logger.Log($"Неизвестная ошибка при добавлении нового сотрудника" +
                                $" {employee.FirstName} {employee.LastName} {employee.MiddleName} табельный номер {employee.TabelNumber} " +
                                $"в NewEmployeesService.Controllers.EmployeeController.AddEmployee: {responseBody}");
                        }

                    }
                    if(alreadyExistEmployees != null)
                    {
                        _ = UpdateEmployeeIds(client, token, alreadyExistEmployees);
                        foreach(var employee in alreadyExistEmployees)
                        {
                            await EditEmployee(client, token, employee);
                        }
                        
                    }
                }
                catch (Exception ex)
                {

                    Logger.Log($"Ошибка добавления новых сотрудников в PercoWeb в NewEmployeesService.Controllers.EmployeeController.AddEmployee: {ex.Message}");
                }
                
            }
        }


        public static async Task GetEmployeeById(HttpClient client, string token, int id)
        {
            var url = $"users/staff/{id}?token={token}";

            var response = await client.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }

     
        public static async Task<List<EmployeeFullListData>?> GetAllEmployeesFromPerco(HttpClient client, string token)
        {
            var url = $"users/staff/fullList?token={token}";
            try
            {
                var response = await client.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                var allEmployees = JsonSerializer.Deserialize<List<EmployeeFullListData>>(responseBody);

                if(allEmployees != null)
                {
                    return allEmployees;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                Logger.Log($"Ошибка получения всех списка всех сотрудников от PercoWeb в NewEmployeesService.Controllers.EmployeeController.GetAllEmployeesFromPerco: {ex.Message}");
                return null;
            }

        }


        public static async Task UpdateEmployeeIds(HttpClient client, string token, List<EmployeeData> employeeWithTabelExist)
        {
            
            var allEmployeesFromPerco = await GetAllEmployeesFromPerco(client, token);
            if(allEmployeesFromPerco != null)
            {
                var employeeEditList = allEmployeesFromPerco.Join(
                employeeWithTabelExist,
                full => full.TabelNumber,
                empTabExist => empTabExist.TabelNumber,
                (full, empTabExist) =>
                {
                    empTabExist.Id = full.Id;
                    return empTabExist;
                });
            }



        }






    }
}
