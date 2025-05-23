using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class VenueEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A venue name is required.")]
        public string VenueName { get; set; } = string.Empty;

        public bool Deleted { get; set; }
    }
}
