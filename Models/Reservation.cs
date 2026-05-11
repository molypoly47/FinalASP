using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservation.Models;

public class Reservation
{
    public int ReservationId { get; set; }

    [Required, Display(Name = "Room")]
    public int RoomId { get; set; }

    [Required, MaxLength(150), Display(Name = "Guest Name")]
    public string GuestName { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Email")]
    public string GuestEmail { get; set; } = string.Empty;

    [Required, Phone, MaxLength(30), Display(Name = "Phone")]
    public string GuestPhone { get; set; } = string.Empty;

    [Required, DataType(DataType.Date), Display(Name = "Check-In Date")]
    public DateTime CheckIn { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Check-Out Date")]
    public DateTime CheckOut { get; set; }

    [Required, Display(Name = "Status")]
    public string Status { get; set; } = "Pending";

    [MaxLength(500), Display(Name = "Special Requests")]
    public string? SpecialRequests { get; set; }

    [Display(Name = "Date Created")]
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public Room? Room { get; set; }

    [NotMapped]
    public int TotalNights => CheckOut > CheckIn ? (CheckOut - CheckIn).Days : 0;

    [NotMapped]
    public decimal TotalAmount => TotalNights * (Room?.PricePerNight ?? 0);

    public static readonly string[] Statuses =
        { "Pending", "Confirmed", "Checked-In", "Checked-Out", "Cancelled" };
}
