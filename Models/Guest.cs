using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterDetailes_HotelManagement_CoreApp.Models
{
    public class Guest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GuestName { get; set; }


        public string? Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? ContactNo { get; set; }

        public string? GuestPicture { get; set; }

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

        public IList<Reservation> reservations { get; set; } = new List<Reservation>();
    }
}
