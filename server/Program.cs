using server.Data; //needed for dapper
using server.Services; //needed for Dependency Injection

/*
Future: Try Entity Framework
*/


/* DapperContext is a helper class that sets your database connection for Dapper, which allows you to run sql commands in c#


Singleton → one instance for the entire app lifetime.
Scoped → one instance per request.
Transient → a brand new instance every time you ask for it.

Dependency Injection is used so you don't have to create instances of your class. AspNetCore injects the needed objects in your class, you just have to declare it in the constructor
*/

var builder = WebApplication.CreateBuilder(args); // Creates a builder object to configure the app’s services, settings, and middleware before it starts running.


// Dependency Injection (DI) registration in ASP.NET Core. ASP.NET Core giving your class what it needs, instead of you creating it yourself.
//this is to register services
builder.Services.AddSingleton<DapperContext>(); //instantiated once. all service share the same dapper context
builder.Services.AddControllers();  //turn on controllers. or else [ApiController] won't work
builder.Services.AddScoped<PostService>(); 
// tells asp.net how PostService is provided whenever it is needed
// AddScoped means a new instance is created for each HTTP request.


var app = builder.Build(); //builds the application


//middlewares
app.UseHttpsRedirection();
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); //connects controller routes


app.Run();