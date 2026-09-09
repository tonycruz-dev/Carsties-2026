
using AuctionService.Data;
using AuctionService.Errors;
using Mapster;
using Microsoft.EntityFrameworkCore;

TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AuctionDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}
app.UseExceptionHandler();


app.MapControllers();

try
{
	DbInitializer.InitDb(app);
}
catch (Exception e)
{
	Console.WriteLine($"Error initializing database: {e.Message}");
}

app.Run();
