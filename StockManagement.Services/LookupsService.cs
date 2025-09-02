using StockManagement.Models;
using StockManagement.Models.Enums;
using StockManagement.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

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
            var actionList = await _actionService.GetAllAsync();
            return new List<LookupsModel>
            {
                new LookupsModel
                {
                    ActionList = actionList.Where(a => a.Id != 1 && a.Id != 5).ToList(), // Exclude 'Add new stock add to' and 'Sale from'
                    ProductList = await _productService.GetAllAsync(),
                    ProductTypeList = await _productTypeService.GetAllAsync(),
                    LocationList = await _locationService.GetAllAsync(),
                    SupplierList = await _contactService.GetByTypeAsync(ContactTypeEnum.Supplier),
                }
            };
        }
    }
}
