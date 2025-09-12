namespace StockManagement.Models.Dto
{
    public class LocationDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int ContactId { get; set; }
    }
}
