using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class ActionEditModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A name is required.")]
        public string ActionName { get; set; } = string.Empty;

        public int Direction { get; set; }
        
        public bool AffectStockRoom { get; set; }
        
        public bool Deleted { get; set; }
    }
}
