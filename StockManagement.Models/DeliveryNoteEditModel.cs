using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class DeliveryNoteEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Venue must be selected.")]
        public int VenueId { get; set; }

        public bool DirectSale { get; set; }

        public bool Deleted { get; set; }
    }
}
