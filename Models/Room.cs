using System.ComponentModel.DataAnnotations;

namespace MasterDetailes_HotelManagement_CoreApp.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        public string Room_Number { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price_Per_Night { get; set; }

        public bool Availability_Status { get; set; }

        public IList<Reservation> reservations { get; set; } = new List<Reservation>();
    }
}
