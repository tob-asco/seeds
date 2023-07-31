using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Api.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddRazorPages();
// register the DbContext with the connection string in "appsettings.json"
builder.Services.AddDbContext<seedsApiContext>(options =>
    options.UseNpgsql(builder.Configuration.
    GetConnectionString("seedsApiContext") ?? throw new InvalidOperationException(
        "Connection string 'seedsApiContext' not found."
)));
// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
Log.Logger = new LoggerConfiguration()
        .WriteTo.File(
            "C:\\Users\\tobia\\source\\repos\\seeds\\log.txt",
            rollingInterval: RollingInterval.Day)
        .CreateLogger();
#endregion

var app = builder.Build();

// Get an instance of the DbContext from the service provider
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<seedsApiContext>();

// Seed data
var dataSeeder = new DataSeeder(dbContext);
dataSeeder.SeedData();

if (app.Environment.IsDevelopment())
{
    // in development mode, we display detailed error messages
    app.UseDeveloperExceptionPage();
}
else
{
    // in not dev. mode, we display a better-looking /Error page
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
// Register the API controllers as endpoints (by GPT):
app.MapControllers();

app.Run();
