using Microsoft.EntityFrameworkCore;

using MontyBackendAPI.Middlewares;
using MontyBackendAPI.Repositories;
using NascodeCMS.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("MyPostgreSQLConnection")));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    
});


var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["LogFilePath"].ToString());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowOrigin");
app.MapControllers();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

