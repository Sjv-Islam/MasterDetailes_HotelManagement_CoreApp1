using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterDetailes_HotelManagement_CoreApp.Models;

namespace MasterDetailes_HotelManagement_CoreApp.Controllers
{
    public class GuestsController : Controller
    {
        private readonly HotelContext _context;
        private readonly IWebHostEnvironment _enc;
        public GuestsController(HotelContext context, IWebHostEnvironment enc)
        {
            _context = context;
            _enc = enc;
        }

        // GET: Guests
        public async Task<IActionResult> Index()
        {
            var data = await _context.Guests.Include(i => i.reservations).ThenInclude(p => p.Room).ToListAsync();


            ViewBag.Count = data.Count;
            ViewBag.GrandTotal = data.Sum(i => i.reservations.Sum(l => l.Total_Cost));

            //ViewBag.Average = data.Average(i=> i.Items.Sum(l=> l.ItemTotal)) ;

            ViewBag.Average = data.Count > 0 ? data.Average(i => i.reservations.Sum(l => l.Total_Cost)) : 0;
            return View(data);
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.Include(i => i.reservations).ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // GET: Guests/Create
        public IActionResult Create()
        {
            return View((new Guest()));
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guest guest, string command = "")
        {
            if (guest.ImageUpload != null)
            {
                guest.GuestPicture = "\\Image\\" + guest.ImageUpload.FileName;

                string serverPath = _enc.WebRootPath + guest.GuestPicture;

                using FileStream stream = new FileStream(serverPath, FileMode.Create);

                await guest.ImageUpload.CopyToAsync(stream);

            }

            if (command == "Add")
            {
                guest.reservations.Add(new());
                return View(guest);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                guest.reservations.RemoveAt(idx);
                ModelState.Clear();
                return View(guest);
            }

            if (ModelState.IsValid)
            {
                //_context.Add(guest);
                //await _context.SaveChangesAsync();                

                var rows = await _context.Database.ExecuteSqlRawAsync("exec spInsertGuest @p0, @p1, @p2, @p3", guest.GuestName, guest.Address, guest.ContactNo, guest.GuestPicture);

                if (rows > 0)
                {
                    guest.Id = _context.Guests.Max(x => x.Id);

                    foreach (var item in guest.reservations)
                    {
                        await _context.Database.ExecuteSqlRawAsync("exec SpInsertReservation @p0, @p1, @p2, @p3", item.RoomId, item.Check_In_Date, item.Check_Out_Date, guest.Id);
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.Include(i => i.reservations).ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Guest guest, string command = "")
        {
            if (id != guest.Id)
            {
                return NotFound();
            }
            if (guest.ImageUpload != null)
            {
                guest.GuestPicture = "\\Image\\" + guest.ImageUpload.FileName;

                string serverPath = _enc.WebRootPath + guest.GuestPicture;

                using FileStream stream = new FileStream(serverPath, FileMode.Create);

                await guest.ImageUpload.CopyToAsync(stream);

            }
            if (command == "Add")
            {
                guest.reservations.Add(new());
                return View(guest);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                guest.reservations.RemoveAt(idx);
                ModelState.Clear();
                return View(guest);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(guest);
                    //await _context.SaveChangesAsync();

                    var rows = await _context.Database.ExecuteSqlRawAsync("exec spUpdateGuest @p0, @p1, @p2, @p3, @p4", guest.Id, guest.GuestName, guest.Address, guest.ContactNo, guest.GuestPicture);

                    foreach (var item in guest.reservations)
                    {
                        await _context.Database.ExecuteSqlRawAsync("exec SpInsertReservation @p0, @p1, @p2, @p3", item.RoomId, item.Check_In_Date, item.Check_Out_Date, guest.Id);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.Include(i => i.reservations).ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var guest = await _context.Guests.FindAsync(id);
            //if (guest != null)
            //{
            //    _context.Guests.Remove(guest);
            //}
            await _context.Database.ExecuteSqlAsync($"exec SpDeleteGuest {id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        [Route("~/deleteguest/{id}")]
        public async Task<IActionResult> DeleteAjax(int id)
        {

            await _context.Database.ExecuteSqlAsync($"exec spDeleteGuest {id}");

            //await _context.SaveChangesAsync();

            return Ok();
        }
        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.Id == id);
        }
    }
}
