using BuildQuote.Domain.Entities;
using BuildQuote.Domain.Repositories;
using BuildQuote.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BuildQuote.Infrastructure.Repositories;

public class SqliteQuoteRepository(AppDbContext context) : IQuoteRepository
{
    public async Task<IEnumerable<Quote>> GetAllAsync() =>
        await context.Quotes.OrderByDescending(q => q.CreatedAt).ToListAsync();

    public async Task<Quote?> GetByIdWithItemsAsync(Guid id) =>
        await context.Quotes
        .Include(q => q.Items)
        .FirstOrDefaultAsync(q => q.Id == id);

    public async Task AddAsync(Quote quote)
    {
        await context.Quotes.AddAsync(quote);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Quote quote)
    {
        quote.UpdatedAt = DateTime.UtcNow;
        context.Quotes.Update(quote);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var quote = await context.Quotes.FindAsync(id);
        if (quote != null)
        {
            context.Quotes.Remove(quote);
            // No cambiamos el IsSynced aquí todavía, lo haremos cuando armemos el motor
            await context.SaveChangesAsync();
        }
    }
}