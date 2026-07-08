using System;
using System.Collections.Generic;
using System.Text;

namespace BuildQuote.Domain.Entities
{
    public class Material
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public UnitOfMeasure Unit { get; set; } = UnitOfMeasure.Unidad;
        public decimal UnitCost { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSynced { get; set; } = false;

    }
}
