namespace StockManagement.Models.Dto
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
    }
}
