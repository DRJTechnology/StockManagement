using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class ProductTypeEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A product type name is required.")]
        public string ProductTypeName { get; set; } = string.Empty;

        public bool Deleted { get; set; }
    }
}
