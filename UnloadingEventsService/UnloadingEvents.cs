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

            string token = await ConnectionService.Connection.GetPercoWebToken(client, Config.Login, Config.Password);

            var jsonEvents = await EventsController.GetRawEvents(client, token); 

            var apiResponse = JsonSerializer.Deserialize<ApiResponse?>(jsonEvents);

            var preparedEvents = EventsController.PreparationForUnloadingEvents(apiResponse!);

            File.WriteAllText(Config.PathToPercoXML, preparedEvents);
        }
    }
}
