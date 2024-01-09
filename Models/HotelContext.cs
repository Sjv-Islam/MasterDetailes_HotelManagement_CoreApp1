using Microsoft.EntityFrameworkCore;

namespace MasterDetailes_HotelManagement_CoreApp.Models
{
    public class HotelContext : DbContext
    {
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public HotelContext(DbContextOptions opt) : base(opt)
        {

        }
    }
}
