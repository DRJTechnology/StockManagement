using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class ContactEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A name is required.")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Type is required.")]
        public int ContactTypeId { get; set; }
        public string Notes { get; set; } = string.Empty;

        public bool Deleted { get; set; }
    }
}
