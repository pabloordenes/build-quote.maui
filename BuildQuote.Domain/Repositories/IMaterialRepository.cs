using BuildQuote.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildQuote.Domain.Repositories
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAllActiveAsync();
        Task<Material?> GetByIdAsync(Guid Id);
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task SoftDeleteAsync(Guid id);
    }
}
