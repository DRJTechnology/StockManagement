using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class ProductProductTypeEditModel
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        public bool Deleted { get; set; }
    }
}
