namespace StockManagement.Models.Dto
{
    public class StockSaleDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        //public bool DirectSale { get; set; }
        public List<StockSaleDetailDto> DetailList { get; set; } = new List<StockSaleDetailDto>();
    }
}
