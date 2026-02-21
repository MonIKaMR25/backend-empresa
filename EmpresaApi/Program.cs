using Microsoft.EntityFrameworkCore;
using EmpresaApi.Data;
using EmpresaApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar el middleware de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseStaticFiles();
    app.UseSwaggerUI(c =>
    {
        c.InjectJavascript("/scripts/swagger-script.js");
    });
}

app.MapGet("/", () => "Hello World!");

// Mapear endpoints de Contactos
app.MapContactosEndpoints();

app.Run();

public partial class Program { }
