var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Start the NewEmployeesMethod as a background task and observe exceptions
_ = Task.Run(() => NewEmployeesService.NewEmployees.NewEmployeesMethod());

app.MapGet("/", () => "Hello World!");

app.Run();
