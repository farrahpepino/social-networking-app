var builder = WebApplication.CreateBuilder(args);


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

