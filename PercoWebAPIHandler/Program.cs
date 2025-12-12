using ConnectionService.Models;
using NewEmployeesService.Models.DTO;
using PercoWebAPIHandler;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


//_ = Task.Run(() => NewEmployeesService.NewEmployees.NewEmployeesMethod());

//_ = Task.Run(() => UnloadingEventsService.UnloadingEvents.Unloading());

//var client = ConnectionService.Connection.client;
//var token = await ConnectionService.Connection.GetPercoWebToken(ConnectionService.Connection.client, ConfigurationService.Config.Login, ConfigurationService.Config.Password);

//_ = Task.Run(() => NewEmployeesService.Controllers.EmployeeController.GetAllEmployeesFromPerco(ConnectionService.Connection.client, token));

await Test.MainTest();



app.MapGet("/", () => "Hello World!");

app.Run();
