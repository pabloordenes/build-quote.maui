using System;
using System.Collections.Generic;
using System.Text;

namespace BuildQuote.Domain.Entities
{
    public class Quote
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClientName { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string ProjectAdress { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddDays(15);
        public List<QuoteItem> Items { get; set; } = [];
        public decimal SubTotal => Items.Sum(i => i.TotalPrice);
        public decimal TaxRate { get; set; } = 0.19m;
        public decimal TaxAmount => SubTotal * TaxRate;
        public decimal Total => SubTotal + TaxAmount;
        public string Status { get; set; } = "Borrador";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSynced { get; set; } = false;

    }
}
