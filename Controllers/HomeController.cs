using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;

namespace RoomReservation.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var rooms        = await _db.Rooms.ToListAsync();
        var reservations = await _db.Reservations.Include(r => r.Room)
                                                 .OrderByDescending(r => r.DateCreated)
                                                 .Take(10)
                                                 .ToListAsync();

        ViewBag.TotalRooms        = rooms.Count;
        ViewBag.AvailableRooms    = rooms.Count(r => r.IsAvailable);
        ViewBag.TotalReservations = await _db.Reservations.CountAsync();
        ViewBag.PendingCount      = await _db.Reservations.CountAsync(r => r.Status == "Pending");

        return View(reservations);
    }
}
