using server.Data; // for dapper
using server.Services; // for Dependency Injection
using server.Models; //for jwtsettings
using server.Middlewares;
using server.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer; //for authentication
using Microsoft.IdentityModel.Tokens; // for signing credentials
using System.Text; // for encoding 
using AwsS3.Models;
using AwsS3.Services;
using Microsoft.AspNetCore.Http.Features;

// DapperContext is a helper class that sets your database connection for Dapper, which allows you to run sql commands in c#


var builder = WebApplication.CreateBuilder(args); 
var secret = builder.Configuration["Jwt:Secret"];

// register services
builder.Services.AddSingleton<DapperContext>(); 
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<AuthService>(); 
builder.Services.AddScoped<UserService>(); 
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<InterestService>(); 
builder.Services.AddScoped<IJwtService, JwtService>(); 
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<CommentRepository>();
builder.Services.AddScoped<InterestRepository>();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddControllers();  
builder.Services.AddSwaggerGen(); 


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
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowAngularDev");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); 
app.UseAuthorization();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.MapControllers(); // connects controller routes

app.Run();