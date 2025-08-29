using server.Data; //needed for dapper
using server.Services; //needed for Dependency Injection
using server.Models; //for jwtsettings
using server.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer; //for authentication
using Microsoft.IdentityModel.Tokens; // for signing credentials
using System.Text; // for encoding 
using AwsS3.Models;
using AwsS3.Services;
using Microsoft.AspNetCore.Http.Features;

/* DapperContext is a helper class that sets your database connection for Dapper, which allows you to run sql commands in c#

Singleton → one instance for the entire app lifetime.
Scoped → one instance per request.
Transient → a brand new instance every time you ask for it.

Dependency Injection is used so you don't have to create instances of your class. AspNetCore injects the needed objects in your class, you just have to declare it in the constructor
*/

//this is for validating JWT for incoming requests

var builder = WebApplication.CreateBuilder(args); // Creates a builder object to configure the app’s services, settings, and middleware before it starts running.
var secret = builder.Configuration["Jwt:Secret"];

// Dependency Injection (DI) registration in ASP.NET Core. ASP.NET Core giving your class what it needs, instead of you creating it yourself.
// AddScoped means a new instance is created for each HTTP request.

// register services
builder.Services.AddSingleton<DapperContext>(); //instantiated once. all service share the same dapper context
builder.Services.AddScoped<PostService>();// tells asp.net how PostService is provided whenever it is needed
builder.Services.AddScoped<AuthService>(); 
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<IJwtService, JwtService>(); //
builder.Services.AddScoped<IStorageService, StorageService>();//
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<CommentRepository>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddControllers();  //turn on controllers. or else [ApiController] won't work
builder.Services.AddSwaggerGen(); // 


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; 
});

//configure cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy
            .WithOrigins("*") 
            .AllowAnyHeader()
            .AllowAnyMethod());
            
});


// configure jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // set true for issuer validation
        ValidateAudience = false, // set true for audience validation
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!))
    };
});



//build app
var app = builder.Build(); 


//configure middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAngularDev");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers(); // connects controller routes

app.Run();