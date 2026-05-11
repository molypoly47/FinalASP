using System.ComponentModel.DataAnnotations;

namespace RoomReservation.Models;

public class Room
{
    public int RoomId { get; set; }

    [Required, MaxLength(100)]
    [Display(Name = "Room Name")]
    public string RoomName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Room Type")]
    public string RoomType { get; set; } = string.Empty;

    [Required, Range(1, 100)]
    public int Capacity { get; set; }

    [Required, Range(0.01, 99999.99)]
    [Display(Name = "Price Per Night (₱)")]
    [DataType(DataType.Currency)]
    public decimal PricePerNight { get; set; }

    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public static readonly string[] RoomTypes =
        { "Standard", "Suite", "Family", "Presidential", "Budget" };
}
