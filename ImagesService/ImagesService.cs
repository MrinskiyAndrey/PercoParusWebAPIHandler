using System;
using System.Collections.Generic;
using System.Text;

namespace ImagesService
{
    public static class ImagesService
    {
        public static async Task MainImagesService()
        {
            var client = ConnectionService.Connection.client;
            var token = await ConnectionService.Connection.GetPercoWebToken(client, ConfigurationService.Config.Login, ConfigurationService.Config.Password);
            
            var id = NewEmployeesService.Controllers.EmployeeController.

        }
    }
}
