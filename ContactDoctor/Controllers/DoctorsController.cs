using ContactDoctor.Data;
using ContactDoctor.DTOs;
using ContactDoctor.Models;
using ContactDoctor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ContactDoctor.Controllers;

public class DoctorsController : Controller
{
    private readonly ContactDoctorContext _context = new();

    // GET
    public async Task<IActionResult> BookAppointment(BookAppointment initialData)
    {
        IQueryable<Doctor> doctors = _context.Doctors;
        var specializations = await doctors
            .Select(d => d.Specialization)
            .Distinct()
            .ToListAsync();

        if (!initialData.Search.IsNullOrEmpty())
        {
            doctors = doctors
                .Where(d => d.Name.Contains(initialData.Search) || d.Specialization.Contains(initialData.Search));
        }

        if (!initialData.Specialization.IsNullOrEmpty())
        {
            doctors = doctors.Where(d => d.Specialization == initialData.Specialization);
        }

        var doctorsCount = doctors.Count();
        var doctorsList = await doctors
            .Skip((initialData.Page - 1) * initialData.PageLimit)
            .Take(initialData.PageLimit)
            .ToListAsync();

        var data = new BookAppointment()
        {
            Doctors = doctorsList,
            Specializations = specializations,
            Search = initialData.Search,
            Specialization = initialData.Specialization,
            Page = initialData.Page,
            PageLimit = initialData.PageLimit,
            PagesCount = (int)Math.Ceiling((decimal)doctorsCount / initialData.PageLimit)
        };

        return View(data);
    }

    public async Task<IActionResult> CompleteAppointment([FromRoute] int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
        {
            return RedirectToAction(nameof(BookAppointment));
        }

        return View(new CompleteAppointment() { Doctor = doctor });
    }

    public async Task<IResult> CompleteAppointmentBooking([FromBody] CreateAppointment appointment)
    {
        var existingAppointment = await _context.Appointments
            .SingleOrDefaultAsync(
                a => a.Time == appointment.AppointmentTime && a.DoctorId == appointment.DoctorId);
        if (existingAppointment != null)
        {
            return Results.BadRequest();
        }

        var newAppointment = new Appointment()
        {
            PatientName = appointment.PatientName,
            Date = appointment.AppointmentDate,
            Time = appointment.AppointmentTime,
            DoctorId = appointment.DoctorId
        };
        await _context.Appointments.AddAsync(newAppointment);
        await _context.SaveChangesAsync();

        return Results.Ok();
    }

    public async Task<IActionResult> Appointments()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Time)
            .ThenBy(a => a.Doctor.Name)
            .ToListAsync();

        return View(new Appointments() { AppointmentsList = appointments });
    }
}