using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models.Finance
{
    public class AccountTypeEditModel
    {
        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;
    }
}
