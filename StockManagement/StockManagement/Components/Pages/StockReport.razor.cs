using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StockManagement.Models.Dto.Reports;
using StockManagement.Services.Interfaces;

namespace StockManagement.Components.Pages
{
    [Authorize]
    public partial class StockReport
    {
        [Inject]
        protected IReportService ReportService { get; set; } = default!;

        private bool IsLoading = true;
        private List<StockReportItemDto> StockReportItems = new();
        private Dictionary<string, Dictionary<string, List<StockReportItemDto>>> GroupedStockReport = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StockReportItems = await ReportService.GetStockReportAsync();
                GroupedStockReport = StockReportItems
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
