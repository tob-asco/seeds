using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using seeds.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddApplicationPart(
        Assembly.Load("seeds.Api")
    ).AddControllersAsServices();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var app = builder.Build();
app.MapControllers();
app.Run();

//give this class a "name", so we can reference it in our TestServer setup
public partial class ProgramTest { }