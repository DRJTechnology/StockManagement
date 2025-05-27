using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StockManagement.Models.Dto.Reports;
using StockManagement.Services.Interfaces;

namespace StockManagement.Components.Pages
{
    [Authorize]
    public partial class SalesReport
    {
        [Inject]
        protected IReportService ReportService { get; set; } = default!;

        private bool IsLoading = true;
        private List<SalesReportItemDto> SalesReportItems = new();
        private Dictionary<string, Dictionary<string, List<SalesReportItemDto>>> GroupedSalesReport = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                SalesReportItems = await ReportService.GetSalesReportAsync();
                GroupedSalesReport = SalesReportItems
                    .GroupBy(item => item.VenueName)
                    .ToDictionary(
                        venueGroup => venueGroup.Key,
                        venueGroup => venueGroup
                            .GroupBy(item => item.ProductTypeName)
                            .ToDictionary(
                                productTypeGroup => productTypeGroup.Key,
                                productTypeGroup => productTypeGroup.ToList()
                            )
                    );
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
