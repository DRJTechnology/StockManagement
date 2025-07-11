using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class StockReceiptEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Supplier must be selected.")]
        public int SupplierId { get; set; }

        public bool Deleted { get; set; }
    }
}
