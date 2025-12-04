using ConfigurationService;
using NewEmployeesService.Models;
using NewEmployeesService.Models.DTO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace NewEmployeesService.Controllers
{
    public class DivisionController
    {
        public static async Task<List<DivisionData>> GetDivisionList(HttpClient client, string host, string token)
        {
            string urlGetDivisions = $"http://{host}/api/divisions/list?token={token}";
            List<DivisionData> divisions = new List<DivisionData>();

            try
            {
                var response = await client.GetAsync(urlGetDivisions);
                var responseBody = await response.Content.ReadAsStringAsync();
                divisions = JsonSerializer.Deserialize<List<DivisionData>>(responseBody) ?? new List<DivisionData>();
                return divisions;
            }
            catch (Exception ex)
            {
                
                Logger.Log($"DivisionController.GetDivisionList: {ex.Message}");
                return divisions;
            }
        }

        public static List<DivisionData> DivisionsIsNotExist(List<DivisionData> divisions, List<EmployeeData> newEmployees)
        {
            

            // получение списков существующих и подразделений и подразделений у новых сотрудников
            var namesInDivisions = divisions.Select(div => div?.Name?.ToString()).ToList();
            var divisionsInEmployees = newEmployees.Select(emp => emp.Division.ToString()).Distinct().ToList();
            // если подразделение null или пустая строка то значение по умолчанию будет 0000
            List<string> preparedDivisionsInNewEmployees = divisionsInEmployees.Select(div => string.IsNullOrEmpty(div) ? "0000" : div).ToList();
            
            // проверка есть ли у новых сотрудников подразделения отсутствующие в Perco
            List<string> divIsNotExistString = preparedDivisionsInNewEmployees.Where(div => !namesInDivisions.Contains(div)).ToList();

            // Создание и заполнение списка новых подразделений
            var divIsNotExist = new List<DivisionData>();

            if (divIsNotExistString.Count > 0)
            {
                foreach (var div in divIsNotExistString)
                {
                    divIsNotExist.Add(new DivisionData { Name = div });
                }
            }

            return divIsNotExist;
        }

        public static void AddDivision(HttpClient client, string host, string token, string newDivision)
        {
            
        }
    }
}
