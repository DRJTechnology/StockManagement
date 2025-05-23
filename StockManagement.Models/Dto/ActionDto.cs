namespace StockManagement.Models.Dto
{
    public class ActionDto
    {
        public int Id { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public int Direction { get; set; }
        public bool AffectStockRoom { get; set; }
        public bool Deleted { get; set; }
    }
}
