using ConfigurationService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using UnloadingEventsService.Models;

namespace UnloadingEventsService.Controllers
{
    public static class EventsController
    {

        public static async Task<string> GetRawEvents(HttpClient client, string token)
        {
            var columns = new List<Columns>
            {
                new Columns {column = "in", value = "1"},
                new Columns {column = "in", value = "2"},
                new Columns {column = "in", value = "3"}
            };

            var filter = new Filter
            {
                type = "or",
                rows = columns,
            };

            var filtersJson = JsonSerializer.Serialize(filter);
            var beginDatetime = DateTime.Now.AddDays(Config.NumberOfDaysEvents * -1);

            string urlGetEvents = $"eventsystem?token={token}&beginDatetime={beginDatetime}&filters={filtersJson}";

            try
            {
                var response = await client.GetAsync(urlGetEvents);
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Logger.Log($"Данные событий не получены {ex.Message}");
                return string.Empty;
            }
        }

        public static string PreparationForUnloadingEvents(ApiResponse apiResponse)
        {
            string strEvents = string.Empty;
            if (apiResponse?.Rows != null)
            {
                foreach (var eventRow in apiResponse.Rows)
                {

                    strEvents += string.Concat(eventRow.TabelNumber, ";",
                        ((eventRow.ZoneEnterId > 1) ? 0 : 1),
                        $";{eventRow.TimeLabel?.Substring(11)};{eventRow.TimeLabel?.Substring(0, eventRow.TimeLabel.Length - 9)};",
                        eventRow.Identifier, Environment.NewLine);
                }
            }
            return strEvents;
        }

    }
}
