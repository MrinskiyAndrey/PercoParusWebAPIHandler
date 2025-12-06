var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


_ = Task.Run(() => NewEmployeesService.NewEmployees.NewEmployeesMethod());

//_ = Task.Run(() => UnloadingEventsService.UnloadingEvents.Unloading());

app.MapGet("/", () => "Hello World!");

app.Run();
