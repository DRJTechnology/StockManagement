using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class SupplierEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A supplier name is required.")]
        public string SupplierName { get; set; } = string.Empty;

        public bool Deleted { get; set; }
    }
}
