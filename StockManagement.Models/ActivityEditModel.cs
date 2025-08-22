using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class ActivityEditModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime ActivityDate { get; set; }

        [Required]
        public int ActionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int DeliveryNoteId { get; set; }

        public int StockOrderId { get; set; }

        public bool Deleted { get; set; }
    }
}
