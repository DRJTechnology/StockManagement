namespace StockManagement.Models.Dto
{
    public class ProductTypeDto
    {
        public int Id { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
    }
}
