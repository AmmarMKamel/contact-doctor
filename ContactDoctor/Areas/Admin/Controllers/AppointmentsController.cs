using ContactDoctor.Data;
using ContactDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactDoctor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly ContactDoctorContext _context = new();

        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .SingleOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
            {
                return View("NotFound");
            }

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            var existingAppointment = await _context.Appointments
                .SingleOrDefaultAsync(
                    a => a.AppointmentDateTime == appointment.AppointmentDateTime && a.DoctorId == appointment.DoctorId);
            if (existingAppointment != null)
            {
                return View("NotFound");
            }

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return View("NotFound");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
