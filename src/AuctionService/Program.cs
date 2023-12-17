using AuctionService.Data;
using AuctionService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuctionService, AuctionServices>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try 
{
    DbInitializer.InitDb(app);
} 
catch(Exception e) 
{
    Console.WriteLine($"An error occurred: {e}");
}

app.Run();
