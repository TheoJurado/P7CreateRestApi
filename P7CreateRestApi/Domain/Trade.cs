using System;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Trade
    {
        [Key]
        public int TradeId { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        public string AccountType { get; set; }
        public double? BuyQuantity { get; set; }
        public double? SellQuantity { get; set; }
        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public DateTime? TradeDate { get; set; }
        [Required]
        public string TradeSecurity { get; set; }
        [Required]
        public string TradeStatus { get; set; }
        [Required]
        public string Trader { get; set; }
        [Required]
        public string Benchmark { get; set; }
        [Required]
        public string Book { get; set; }
        [Required]
        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        [Required]
        public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        [Required]
        public string DealName { get; set; }
        [Required]
        public string DealType { get; set; }
        [Required]
        public string SourceListId { get; set; }
        [Required]
        public string Side { get; set; }
    }
}