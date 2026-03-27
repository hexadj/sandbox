using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
var databaseUrl = builder.Configuration["DATABASE_URL"];
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    connectionString = DatabaseUrlParser.ToNpgsqlConnectionString(databaseUrl);
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "missing database connection string. set ConnectionStrings__Default or DATABASE_URL.");
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<CounterService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost",
                "http://127.0.0.1",
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "http://localhost:4173",
                "http://localhost:80",
                "http://frontend",
                "http://frontend:80")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await CounterService.EnsureSeedAsync(db);
}

app.UseCors("FrontendPolicy");

app.MapGet("/counter", async (CounterService counterService, CancellationToken cancellationToken) =>
{
    var value = await counterService.GetValueAsync(cancellationToken);
    return Results.Ok(new { value });
});

app.MapPost("/counter/increment", async (CounterService counterService, CancellationToken cancellationToken) =>
{
    var value = await counterService.IncrementAsync(cancellationToken);
    return Results.Ok(new { value });
});

app.Run();
