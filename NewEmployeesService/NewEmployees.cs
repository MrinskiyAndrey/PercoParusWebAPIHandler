using NewEmployeesService.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewEmployeesService
{
    public class NewEmployees
    {
        public static async Task NewEmployeesMethod()
        {
            var client = ConnectionService.Connection.client;
            var config = new ConfigurationService.Config();
            var token = await ConnectionService.Connection.GetPercoWebToken(config.PercoWebHost, client, config.Login, config.Password);

            string viewContent = await ViewReaderController.ReadViewAsync(config.PathToNewEmployeesView);
            var newEmployees = ViewReaderController.GetNewEmployee(viewContent);

            //foreach (var employee in newEmployees)
            //{
            //    Console.WriteLine($"Adding new employee:{employee.Position} {employee.Division} {employee.TabelNumber} {employee.FirstName} {employee.LastName} {employee.MiddleName}");
            //}
            var divList = await DivisionController.GetDivisionList(client, config.PercoWebHost, token);

            var divsIsNotExist = DivisionController.DivisionsIsNotExist(divList, newEmployees);

            foreach (var div in divsIsNotExist)
            {
                Console.WriteLine($"Подразделение : {div.Name}");
            }
        }
    }
}
