var builder = WebApplication.CreateBuilder(args);

// Dependency Injection (DI) registration in ASP.NET Core.
builder.Services.AddSingleton<DapperContext>();

var app = builder.Build();


//middlewares
app.UseHttpsRedirection();
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();


app.Run();

