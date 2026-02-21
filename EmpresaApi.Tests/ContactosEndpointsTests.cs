using System.Net;
using System.Net.Http.Json;
using EmpresaApi.Data;
using EmpresaApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmpresaApi.Tests;

public class ContactosEndpointsTests : IClassFixture<ContactosWebApplicationFactory>
{
    private readonly ContactosWebApplicationFactory _factory;

    public ContactosEndpointsTests(ContactosWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient()
    {
        var client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Contactos.RemoveRange(db.Contactos);
        db.SaveChanges();
        return client;
    }

    // ─── GET /api/contactos ──────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerContactos_RetornaListaVacia_CuandoNoHayContactos()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/contactos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var lista = await response.Content.ReadFromJsonAsync<List<Contacto>>();
        Assert.NotNull(lista);
        Assert.Empty(lista);
    }

    [Fact]
    public async Task ObtenerContactos_RetornaContactos_CuandoExisten()
    {
        var client = CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Contactos.Add(new Contacto { Nombre = "Ana", Apellido = "García", Telefono = "1234567890", Correo = "ana@example.com" });
        db.Contactos.Add(new Contacto { Nombre = "Luis", Apellido = "Pérez", Telefono = "0987654321", Correo = "luis@example.com" });
        await db.SaveChangesAsync();

        var response = await client.GetAsync("/api/contactos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var lista = await response.Content.ReadFromJsonAsync<List<Contacto>>();
        Assert.NotNull(lista);
        Assert.Equal(2, lista.Count);
    }

    // ─── GET /api/contactos/{id} ─────────────────────────────────────────────

    [Fact]
    public async Task ObtenerContactoPorId_RetornaContacto_CuandoExiste()
    {
        var client = CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var contacto = new Contacto { Nombre = "Ana", Apellido = "García", Telefono = "1234567890", Correo = "ana@example.com" };
        db.Contactos.Add(contacto);
        await db.SaveChangesAsync();

        var response = await client.GetAsync($"/api/contactos/{contacto.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var resultado = await response.Content.ReadFromJsonAsync<Contacto>();
        Assert.NotNull(resultado);
        Assert.Equal("Ana", resultado.Nombre);
        Assert.Equal("García", resultado.Apellido);
    }

    [Fact]
    public async Task ObtenerContactoPorId_RetornaNotFound_CuandoNoExiste()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/contactos/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ─── POST /api/contactos ─────────────────────────────────────────────────

    [Fact]
    public async Task AgregarContacto_RetornaCreated_CuandoDatosValidos()
    {
        var client = CreateClient();
        var nuevo = new Contacto { Nombre = "Carlos", Apellido = "López", Telefono = "5551234567", Correo = "carlos@example.com" };

        var response = await client.PostAsJsonAsync("/api/contactos", nuevo);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var creado = await response.Content.ReadFromJsonAsync<Contacto>();
        Assert.NotNull(creado);
        Assert.True(creado.Id > 0);
        Assert.Equal("Carlos", creado.Nombre);
        Assert.Equal("carlos@example.com", creado.Correo);
    }

    [Fact]
    public async Task AgregarContacto_RetornaBadRequest_CuandoNombreEsVacio()
    {
        var client = CreateClient();
        var invalido = new Contacto { Nombre = "", Apellido = "López", Telefono = "5551234567", Correo = "carlos@example.com" };

        var response = await client.PostAsJsonAsync("/api/contactos", invalido);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AgregarContacto_RetornaBadRequest_CuandoCorreoEsInvalido()
    {
        var client = CreateClient();
        var invalido = new Contacto { Nombre = "Carlos", Apellido = "López", Telefono = "5551234567", Correo = "correo-invalido" };

        var response = await client.PostAsJsonAsync("/api/contactos", invalido);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ─── PUT /api/contactos/{id} ─────────────────────────────────────────────

    [Fact]
    public async Task ActualizarContacto_RetornaOk_CuandoDatosValidosYExiste()
    {
        var client = CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var contacto = new Contacto { Nombre = "Pedro", Apellido = "Ruiz", Telefono = "1112223333", Correo = "pedro@example.com" };
        db.Contactos.Add(contacto);
        await db.SaveChangesAsync();

        var actualizado = new Contacto { Nombre = "PedroNuevo", Apellido = "RuizNuevo", Telefono = "9998887777", Correo = "pedro.nuevo@example.com" };

        var response = await client.PutAsJsonAsync($"/api/contactos/{contacto.Id}", actualizado);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var resultado = await response.Content.ReadFromJsonAsync<Contacto>();
        Assert.NotNull(resultado);
        Assert.Equal("PedroNuevo", resultado.Nombre);
        Assert.Equal("RuizNuevo", resultado.Apellido);
        Assert.Equal("pedro.nuevo@example.com", resultado.Correo);
    }

    [Fact]
    public async Task ActualizarContacto_RetornaNotFound_CuandoNoExiste()
    {
        var client = CreateClient();
        var actualizado = new Contacto { Nombre = "Juan", Apellido = "Díaz", Telefono = "1231231234", Correo = "juan@example.com" };

        var response = await client.PutAsJsonAsync("/api/contactos/9999", actualizado);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ActualizarContacto_RetornaBadRequest_CuandoDatosInvalidos()
    {
        var client = CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var contacto = new Contacto { Nombre = "Pedro", Apellido = "Ruiz", Telefono = "1112223333", Correo = "pedro@example.com" };
        db.Contactos.Add(contacto);
        await db.SaveChangesAsync();

        var invalido = new Contacto { Nombre = "", Apellido = "Ruiz", Telefono = "1112223333", Correo = "pedro@example.com" };

        var response = await client.PutAsJsonAsync($"/api/contactos/{contacto.Id}", invalido);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ─── DELETE /api/contactos/{id} ──────────────────────────────────────────

    [Fact]
    public async Task EliminarContacto_RetornaNoContent_CuandoExiste()
    {
        var client = CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var contacto = new Contacto { Nombre = "María", Apellido = "Torres", Telefono = "4445556666", Correo = "maria@example.com" };
        db.Contactos.Add(contacto);
        await db.SaveChangesAsync();

        var response = await client.DeleteAsync($"/api/contactos/{contacto.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task EliminarContacto_RetornaNotFound_CuandoNoExiste()
    {
        var client = CreateClient();

        var response = await client.DeleteAsync("/api/contactos/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EliminarContacto_EliminaElContacto_DeLaBaseDeDatos()
    {
        var client = CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var contacto = new Contacto { Nombre = "Rosa", Apellido = "Mendez", Telefono = "7778889999", Correo = "rosa@example.com" };
        db.Contactos.Add(contacto);
        await db.SaveChangesAsync();
        var id = contacto.Id;

        await client.DeleteAsync($"/api/contactos/{id}");

        var getResponse = await client.GetAsync($"/api/contactos/{id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

