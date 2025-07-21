namespace StockManagement.Models.Dto.Finance
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int AccountTypeId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
}
