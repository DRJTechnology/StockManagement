namespace StockManagement.Models.Dto
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool Deleted { get; set; }
    }
}
