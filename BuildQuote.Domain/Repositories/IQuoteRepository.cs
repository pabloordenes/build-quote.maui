using BuildQuote.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildQuote.Domain.Repositories
{
    public interface IQuoteRepository
    {
        Task<IEnumerable<Quote>> GetAllAsync();
        Task<Quote?> GetByIdWithItemsAsync(Guid id);
        Task AddAsync(Quote quote);
        Task UpdateAsync(Quote quote);
        Task DeleteAsync(Guid id);
    }
}
