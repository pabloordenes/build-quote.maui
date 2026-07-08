using System;
using System.Collections.Generic;
using System.Text;

namespace BuildQuote.Domain.Entities
{
    public class QuoteItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuoteId { get; set; }
        public Guid MaterialId { get; set; }

        public string DescriptionOverride { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public bool IsSynced { get; set; } = false;
    }
}
