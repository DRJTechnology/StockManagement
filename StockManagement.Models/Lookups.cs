namespace StockManagement.Models
{
    public class LookupsModel
    {
        public List<ProductResponseModel> ProductList { get; set; }
        public List<ProductTypeResponseModel> ProductTypeList { get; set; }
        public List<LocationResponseModel> LocationList { get; set; }
        public List<ActionResponseModel> ActionList { get; set; }
        public List<ContactResponseModel> SupplierList { get; set; }
    }
}
