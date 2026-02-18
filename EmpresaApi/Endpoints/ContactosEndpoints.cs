using Microsoft.EntityFrameworkCore;
using EmpresaApi.Data;
using EmpresaApi.Models;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApi.Endpoints;

public static class ContactosEndpoints
{
    private static IResult? ValidateContacto(Contacto contacto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(contacto);
        
        if (!Validator.TryValidateObject(contacto, validationContext, validationResults, true))
        {
            var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { errors });
        }
        
        return null;
    }

    public static void MapContactosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/contactos").WithTags("Contactos");

        // Obtener todos los contactos
        group.MapGet("/", async (AppDbContext db) =>
        {
            var contactos = await db.Contactos.ToListAsync();
            return Results.Ok(contactos);
        })
        .WithName("ObtenerContactos");

        // Obtener un contacto por ID
        group.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var contacto = await db.Contactos.FindAsync(id);
            return contacto is not null ? Results.Ok(contacto) : Results.NotFound();
        })
        .WithName("ObtenerContactoPorId");

        // Agregar un nuevo contacto
        group.MapPost("/", async (Contacto contacto, AppDbContext db) =>
        {
            var validationError = ValidateContacto(contacto);
            if (validationError is not null)
                return validationError;
            
            db.Contactos.Add(contacto);
            await db.SaveChangesAsync();
            return Results.Created($"/api/contactos/{contacto.Id}", contacto);
        })
        .WithName("AgregarContacto");

        // Actualizar un contacto
        group.MapPut("/{id}", async (int id, Contacto contactoActualizado, AppDbContext db) =>
        {
            var validationError = ValidateContacto(contactoActualizado);
            if (validationError is not null)
                return validationError;
            
            var contacto = await db.Contactos.FindAsync(id);
            if (contacto is null)
                return Results.NotFound();

            contacto.Nombre = contactoActualizado.Nombre;
            contacto.Apellido = contactoActualizado.Apellido;
            contacto.Telefono = contactoActualizado.Telefono;
            contacto.Correo = contactoActualizado.Correo;

            await db.SaveChangesAsync();
            return Results.Ok(contacto);
        })
        .WithName("ActualizarContacto");

        // Eliminar un contacto
        group.MapDelete("/{id}", async (int id, AppDbContext db) =>
        {
            var contacto = await db.Contactos.FindAsync(id);
            if (contacto is null)
                return Results.NotFound();

            db.Contactos.Remove(contacto);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("EliminarContacto");
    }
}
