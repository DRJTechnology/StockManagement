using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models.Finance
{
    public class AccountEditModel
    {
        public int Id { get; set; }

        public int AccountTypeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A account name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public bool Active { get; set; } = true;

        public bool Deleted { get; set; }
    }
}
