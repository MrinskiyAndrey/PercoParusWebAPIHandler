using ConnectionService.Models;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


//_ = Task.Run(() => NewEmployeesService.NewEmployees.NewEmployeesMethod());

//_ = Task.Run(() => UnloadingEventsService.UnloadingEvents.Unloading());

var token = await ConnectionService.Connection.GetPercoWebToken(ConnectionService.Connection.client, ConfigurationService.Config.Login, ConfigurationService.Config.Password);

_ = Task.Run(() => NewEmployeesService.Controllers.EmployeeController.GetAllEmployeesFromPerco(ConnectionService.Connection.client, token));





app.MapGet("/", () => "Hello World!");

app.Run();
