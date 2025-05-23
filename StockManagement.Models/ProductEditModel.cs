using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class ProductEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A product name is required.")]
        public string ProductName { get; set; } = string.Empty;

        public bool Deleted { get; set; }
    }
}
