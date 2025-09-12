using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class StockSaleDetailEditModel
    {
        public int Id { get; set; }

        public int StockSaleId { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Type is required.")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool Deleted { get; set; }
    }
}
