using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class StockSaleEditModel
    {
        public int Id { get; set; }

        public int StockSaleId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Location must be selected.")]
        public int LocationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Customer must be selected.")]
        public int ContactId { get; set; }
        public decimal TotalPrice { get; set; }

        public bool SaleConfirmed { get; set; }

        public bool PaymentReceived { get; set; }

        public bool Deleted { get; set; }
    }
}
