namespace StockManagement.Models.Dto
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int AmendUserId { get; set; }
        public DateTime AmendDate { get; set; }
    }
}
