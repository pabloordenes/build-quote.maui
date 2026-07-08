using BuildQuote.Domain.Entities;
using BuildQuote.Domain.Repositories;
using BuildQuote.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BuildQuote.Infrastructure.Repositories;

public class SqliteMaterialRepository(AppDbContext context) : IMaterialRepository
{
    public async Task<IEnumerable<Material>> GetAllActiveAsync() =>
        await context.Materials.Where(m => m.IsActive).ToListAsync();

    public async Task<Material?> GetByIdAsync(Guid id) =>
        await context.Materials.FindAsync(id);

    public async Task AddAsync(Material material)
    {
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Material material)
    {
        material.UpdatedAt = DateTime.UtcNow;
        context.Materials.Update(material);
        await context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var material = await context.Materials.FindAsync(id);
        if (material != null)
        {
            material.IsActive = false;
            material.UpdatedAt = DateTime.UtcNow;
            // No cambiamos el IsSynced aquí todavía, lo haremos cuando armemos el motor
            await context.SaveChangesAsync();
        }
    }
}