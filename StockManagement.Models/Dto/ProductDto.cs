namespace StockManagement.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
    }
}
