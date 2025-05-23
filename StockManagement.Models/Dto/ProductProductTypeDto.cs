namespace StockManagement.Models.Dto
{
    public class ProductProductTypeDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public bool Deleted { get; set; }
    }
}
