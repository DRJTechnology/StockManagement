using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class SettingEditModel
    {
        public int Id { get; set; }

        public string SettingName { get; set; } = string.Empty;

        public string SettingValue { get; set; } = string.Empty;

        public bool Deleted { get; set; }
    }
}
