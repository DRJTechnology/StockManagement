using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models
{
    public class DeliveryNoteEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }

        public bool Deleted { get; set; }
    }
}
