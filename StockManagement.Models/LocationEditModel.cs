using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class LocationEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A name is required.")]
        public string Name { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int ContactId { get; set; }
        public bool Deleted { get; set; }
    }
}
