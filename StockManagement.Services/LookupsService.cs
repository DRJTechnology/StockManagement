using StockManagement.Models;
using StockManagement.Models.Emuns;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class LookupsService : ILookupsService
    {
        private readonly IActionService _actionService;
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly IContactService _contactService;
        private readonly ILocationService _locationService;

        public LookupsService(
            IActionService actionService,
            IProductService productService,
            IProductTypeService productTypeService,
            IContactService contactService,
            ILocationService locationService)
        {
            _actionService = actionService;
            _productService = productService;
            _productTypeService = productTypeService;
            _contactService = contactService;
            _locationService = locationService;
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
                    LocationList = await _locationService.GetAllAsync(),
                    SupplierList = await _contactService.GetByTypeAsync(ContactTypeEnum.Supplier),
                }
            };
        }
    }
}
