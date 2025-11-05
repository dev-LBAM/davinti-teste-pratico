using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using TelephoneDiary.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

builder.Services.AddDbContext<AgendaContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
