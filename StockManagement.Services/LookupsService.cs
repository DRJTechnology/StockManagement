using StockManagement.Models;
using StockManagement.Services.Interfaces;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Services
{
    public class LookupsService : ILookupsService
    {
        private readonly IActionService _actionService;
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly IVenueService _venueService;
        private readonly ISupplierService _supplierService;

        public LookupsService(
            IActionService actionService,
            IProductService productService,
            IProductTypeService productTypeService,
            IVenueService venueService,
            ISupplierService supplierService)
        {
            _actionService = actionService;
            _productService = productService;
            _productTypeService = productTypeService;
            _venueService = venueService;
            _supplierService = supplierService;
        }

        public async Task<List<LookupsModel>> GetLookupsAsync()
        {
            return new List<LookupsModel>
            {
                new LookupsModel
                {
                    ActionList = await _actionService.GetAllAsync(),
                    ProductList = await _productService.GetAllAsync(),
                    ProductTypeList = await _productTypeService.GetAllAsync(),
                    VenueList = await _venueService.GetAllAsync(),
                    SupplierList = await _supplierService.GetAllAsync(),
                }
            };
        }
    }
}
