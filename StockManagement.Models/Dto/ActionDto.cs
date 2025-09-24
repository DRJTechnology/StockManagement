namespace StockManagement.Models.Dto
{
    public class ActionDto : BaseDto
    {
        public string ActionName { get; set; } = string.Empty;
        public int Direction { get; set; }
        public bool AffectStockRoom { get; set; }
    }
}
