using ConfigurationService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using UnloadingEventsService.Controllers;
using UnloadingEventsService.Models;

namespace UnloadingEventsService
{
    public static class UnloadingEvents
    {
        public static async Task Unloading()
        {
            var client = ConnectionService.Connection.client;
            var config = new Config();

            string token = await ConnectionService.Connection.GetPercoWebToken(config.PercoWebHost, client, config.Login, config.Password);

            var jsonEvents = await EventsController.GetRawEvents(config.PercoWebHost, client, token); 

            var apiResponse = JsonSerializer.Deserialize<ApiResponse?>(jsonEvents);

            var preparedEvents = EventsController.PreparationForUnloadingEvents(apiResponse!);

            File.WriteAllText(config.PathToPercoXML, preparedEvents);
        }
    }
}
