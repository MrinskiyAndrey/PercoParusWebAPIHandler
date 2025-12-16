using NewEmployeesService.Models;
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




            


        }
    }
}
