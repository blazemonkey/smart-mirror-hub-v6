using Microsoft.OpenApi.Models;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Smart Mirror Hub API",
            Version = "v1"
        }
     );
});

var types = typeof(BaseRepository).Assembly.GetTypes().Where(x => x.BaseType == typeof(BaseRepository));
foreach (var type in types)
{
    var interfaceType = type.GetInterface($"I{type.Name}");
    if (interfaceType == null)
        continue;

    builder.Services.AddTransient(interfaceType, type);
}

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();
var unitOfWork = app.Services.GetService<IUnitOfWork>();

// if it's more than 1, then its the swagger tofile tool generating the JSON file
if (args.Length == 0)
{
    var components = Component.GetComponents();
    await unitOfWork.Components.ReplaceAll(components);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
