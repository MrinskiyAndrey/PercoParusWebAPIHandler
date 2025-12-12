using NewEmployeesService.Models.DTO;
using System.Text;
using System.Text.Json;

namespace PercoWebAPIHandler
{
    public static class Test
    {
        public static async Task MainTest()
        {

            var client = ConnectionService.Connection.client;
            var token = await ConnectionService.Connection.GetPercoWebToken(ConnectionService.Connection.client, ConfigurationService.Config.Login, ConfigurationService.Config.Password);

            var emp = new EmployeeData
            {
                LastName = "Андрей",
                FirstName = "Мринский",
                MiddleName = "Николаевич",
                TabelNumber = "T17486",
                HiringDate = "2019-12-10",
                Division = 32,
                Position = 20,
            };


            var url = $"users/staff/1391?token={token}";

            var jsonEmp = JsonSerializer.Serialize(emp);
            var content = new StringContent(jsonEmp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);


        }
    }
}
