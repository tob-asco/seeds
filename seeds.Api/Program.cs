using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
