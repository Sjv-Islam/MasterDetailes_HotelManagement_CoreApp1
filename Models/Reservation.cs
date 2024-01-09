using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterDetailes_HotelManagement_CoreApp.Models
{
    public class Reservation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Reservation_ID { get; set; }
        [Required]
        public int? RoomId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Check_In_Date { get; set; } = DateTime.Today;


        public DateTime Check_Out_Date { get; set; } = DateTime.Today.AddDays(1);


        public decimal Total_Cost => this.Room is null ? 0 : this.Room.Price_Per_Night * (this.Check_Out_Date - this.Check_In_Date).Days;

        public int? GuestId { get; set; }


        public Room? Room { get; set; }
        public Guest? Guest { get; set; }
    }
}
