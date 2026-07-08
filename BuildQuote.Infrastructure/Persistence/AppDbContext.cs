using BuildQuote.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildQuote.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<QuoteItem> QuoteItems => Set<QuoteItem>();

    // El constructor recibe las opciones desde la inyección de dependencias (MauiProgram)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Material
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(200);

            // SQLite guarda los Enums como números por defecto, lo cual es perfecto.
            entity.Property(m => m.Unit).IsRequired();
        });

        // 2. Configuración de Quote (Presupuesto)
        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(q => q.Id);

            // Ignoramos las propiedades calculadas para que EF Core no intente 
            // crearles una columna en SQLite (ya que se calculan en memoria).
            entity.Ignore(q => q.SubTotal);
            entity.Ignore(q => q.TaxAmount);
            entity.Ignore(q => q.Total);
        });

        // 3. Configuración de QuoteItem y Relaciones
        modelBuilder.Entity<QuoteItem>(entity =>
        {
            entity.HasKey(qi => qi.Id);

            // Relación: Un Presupuesto tiene muchos Ítems
            entity.HasOne<Quote>()
                  .WithMany(q => q.Items)
                  .HasForeignKey(qi => qi.QuoteId)
                  .OnDelete(DeleteBehavior.Cascade); // Si borras la Quote, se borran sus QuoteItems

            // Relación: Un Ítem hace referencia a un Material (Pero sin cascada)
            // Si borramos un material, no queremos que se borre el ítem del presupuesto histórico.
            entity.HasOne<Material>()
                  .WithMany()
                  .HasForeignKey(qi => qi.MaterialId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}