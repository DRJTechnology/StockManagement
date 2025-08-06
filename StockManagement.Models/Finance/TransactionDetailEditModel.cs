using StockManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models.Finance
{
    public class TransactionDetailEditModel
    {
        public int Id { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }

        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A description is required.")]
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public Int16 Direction { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public int? ContactId { get; set; }
    }
}
