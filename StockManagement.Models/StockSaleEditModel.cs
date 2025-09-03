using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class StockSaleEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Location must be selected.")]
        public int LocationId { get; set; }

        public bool DirectSale { get; set; }

        public bool Deleted { get; set; }
    }
}
