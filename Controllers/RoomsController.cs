using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;

namespace RoomReservation.Controllers;

public class RoomsController : Controller
{
    private readonly ApplicationDbContext _db;
    public RoomsController(ApplicationDbContext db) => _db = db;

    // GET: Rooms
    public async Task<IActionResult> Index()
    {
        var rooms = await _db.Rooms
            .Include(r => r.Reservations)
            .OrderBy(r => r.RoomName)
            .ToListAsync();
        return View(rooms);
    }

    // GET: Rooms/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var room = await _db.Rooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.RoomId == id);
        if (room == null) return NotFound();
        return View(room);
    }

    // GET: Rooms/Create
    public IActionResult Create()
    {
        ViewBag.RoomTypes = Room.RoomTypes;
        return View();
    }

    // POST: Rooms/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Room room)
    {
        if (ModelState.IsValid)
        {
            _db.Add(room);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Room \"{room.RoomName}\" was created successfully.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.RoomTypes = Room.RoomTypes;
        return View(room);
    }

    // GET: Rooms/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room == null) return NotFound();
        ViewBag.RoomTypes = Room.RoomTypes;
        return View(room);
    }

    // POST: Rooms/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Room room)
    {
        if (id != room.RoomId) return BadRequest();
        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(room);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Room \"{room.RoomName}\" was updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Rooms.AnyAsync(r => r.RoomId == id)) return NotFound();
                throw;
            }
        }
        ViewBag.RoomTypes = Room.RoomTypes;
        return View(room);
    }

    // GET: Rooms/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _db.Rooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.RoomId == id);
        if (room == null) return NotFound();
        return View(room);
    }

    // POST: Rooms/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room != null)
        {
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Room \"{room.RoomName}\" was deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
