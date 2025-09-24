namespace StockManagement.Models
{
    public class LookupsModel
    {
        public List<ProductResponseModel> ProductList { get; set; } = new();
        public List<ProductTypeResponseModel> ProductTypeList { get; set; } = new();
        public List<LocationResponseModel> LocationList { get; set; } = new();
        public List<ActionResponseModel> ActionList { get; set; } = new();
        public List<ContactResponseModel> SupplierList { get; set; } = new();
    }
}
