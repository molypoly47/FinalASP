using Microsoft.EntityFrameworkCore;
using RoomReservation.Models;

namespace RoomReservation.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>().HasData(
            new Room { RoomId = 1, RoomName = "Ocean View 101", RoomType = "Standard",    Capacity = 2, PricePerNight = 2500.00m, IsAvailable = true,  Description = "Bright standard room with a beautiful ocean view and king bed." },
            new Room { RoomId = 2, RoomName = "Sky Suite 201",  RoomType = "Suite",       Capacity = 3, PricePerNight = 6500.00m, IsAvailable = true,  Description = "Luxurious suite with panoramic skyline view, jacuzzi, and living area." },
            new Room { RoomId = 3, RoomName = "Family Nest 301",RoomType = "Family",      Capacity = 5, PricePerNight = 4200.00m, IsAvailable = false, Description = "Spacious family room with two queen beds and a pull-out sofa." },
            new Room { RoomId = 4, RoomName = "Royal Crown 401",RoomType = "Presidential",Capacity = 4, PricePerNight = 15000.00m,IsAvailable = true,  Description = "The pinnacle of luxury — private terrace, butler service, and private pool." },
            new Room { RoomId = 5, RoomName = "Cozy Budget 102",RoomType = "Budget",      Capacity = 1, PricePerNight = 1200.00m, IsAvailable = true,  Description = "Comfortable and affordable room perfect for solo travelers." }
        );

        modelBuilder.Entity<Reservation>().HasData(
            new Reservation
            {
                ReservationId = 1, RoomId = 1, GuestName = "Maria Santos",
                GuestEmail = "maria.santos@email.com", GuestPhone = "09171234567",
                CheckIn = new DateTime(2025, 6, 10), CheckOut = new DateTime(2025, 6, 14),
                Status = "Confirmed", SpecialRequests = "Early check-in if possible.",
                DateCreated = new DateTime(2025, 5, 20)
            },
            new Reservation
            {
                ReservationId = 2, RoomId = 2, GuestName = "John Reyes",
                GuestEmail = "john.reyes@email.com", GuestPhone = "09189876543",
                CheckIn = new DateTime(2025, 6, 12), CheckOut = new DateTime(2025, 6, 15),
                Status = "Pending", SpecialRequests = "Airport transfer needed.",
                DateCreated = new DateTime(2025, 5, 22)
            },
            new Reservation
            {
                ReservationId = 3, RoomId = 4, GuestName = "Ana Lim",
                GuestEmail = "ana.lim@email.com", GuestPhone = "09201112233",
                CheckIn = new DateTime(2025, 6, 20), CheckOut = new DateTime(2025, 6, 25),
                Status = "Checked-In", SpecialRequests = "Champagne on arrival, extra pillows.",
                DateCreated = new DateTime(2025, 5, 25)
            }
        );
    }
}
