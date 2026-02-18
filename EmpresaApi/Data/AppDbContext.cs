using Microsoft.EntityFrameworkCore;
using EmpresaApi.Models;

namespace EmpresaApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Contacto> Contactos { get; set; }
}
