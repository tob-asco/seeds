using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using seeds.Api.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// register the DbContext with the connection string in "appsettings.json"
builder.Services.AddDbContext<seedsApiContext>(options =>
    options.UseNpgsql(builder.Configuration.
    GetConnectionString("seedsApiContext") ?? throw new InvalidOperationException(
        "Connection string 'seedsApiContext' not found."
)));


var app = builder.Build();

// Get an instance of the DbContext from the service provider
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<seedsApiContext>();

// Seed data
var dataSeeder = new DataSeeder(dbContext);
dataSeeder.SeedData();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
// Register the API controllers as endpoints (by GPT):
app.MapControllers();

app.Run();
