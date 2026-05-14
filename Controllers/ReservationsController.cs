using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;

namespace RoomReservation.Controllers;

public class ReservationsController : Controller
{
    private readonly ApplicationDbContext _db;
    public ReservationsController(ApplicationDbContext db) => _db = db;

    // GET: Reservations
    public async Task<IActionResult> Index(string? search, string? status)
    {
        ViewBag.CurrentSearch = search;
        ViewBag.CurrentStatus = status;
        ViewBag.Statuses = Reservation.Statuses;

        var query = _db.Reservations.Include(r => r.Room).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.GuestName.Contains(search) || r.GuestEmail.Contains(search));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status == status);

        var reservations = await query.OrderByDescending(r => r.DateCreated).ToListAsync();
        return View(reservations);
    }

    // GET: Reservations/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _db.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.ReservationId == id);
        if (reservation == null) return NotFound();
        return View(reservation);
    }

    // GET: Reservations/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Rooms    = await _db.Rooms.Where(r => r.IsAvailable).OrderBy(r => r.RoomName).ToListAsync();
        ViewBag.Statuses = Reservation.Statuses;
        return View();
    }

    // POST: Reservations/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        if (reservation.CheckOut <= reservation.CheckIn)
            ModelState.AddModelError("CheckOut", "Check-out date must be after check-in date.");

        if (ModelState.IsValid)
        {
            reservation.DateCreated = DateTime.Now;
            _db.Add(reservation);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Reservation for \"{reservation.GuestName}\" was created successfully.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Rooms    = await _db.Rooms.Where(r => r.IsAvailable).OrderBy(r => r.RoomName).ToListAsync();
        ViewBag.Statuses = Reservation.Statuses;
        return View(reservation);
    }

    // GET: Reservations/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation == null) return NotFound();
        ViewBag.Rooms    = await _db.Rooms.OrderBy(r => r.RoomName).ToListAsync();
        ViewBag.Statuses = Reservation.Statuses;
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Reservation reservation)
    {
        if (id != reservation.ReservationId) return BadRequest();

        if (reservation.CheckOut <= reservation.CheckIn)
            ModelState.AddModelError("CheckOut", "Check-out date must be after check-in date.");

        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(reservation);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Reservation for \"{reservation.GuestName}\" was updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Reservations.AnyAsync(r => r.ReservationId == id)) return NotFound();
                throw;
            }
        }

        ViewBag.Rooms    = await _db.Rooms.OrderBy(r => r.RoomName).ToListAsync();
        ViewBag.Statuses = Reservation.Statuses;
        return View(reservation);
    }

    // GET: Reservations/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _db.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.ReservationId == id);
        if (reservation == null) return NotFound();
        return View(reservation);
    }

    // POST: Reservations/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Reservation for \"{reservation.GuestName}\" was deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}
