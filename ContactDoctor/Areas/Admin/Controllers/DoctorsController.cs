using ContactDoctor.Data;
using ContactDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactDoctor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorsController : Controller
    {
        private readonly ContactDoctorContext _context = new();

        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors.ToListAsync();

            return View(doctors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor, IFormFile? img)
        {
            if (img != null && img?.Length > 0)
            {
                var ext = Path.GetExtension(img.FileName).ToLowerInvariant();
                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await img.CopyToAsync(stream);
                    doctor.Img = fileName;
                }
            }

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return View("NotFound");
            }

            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Doctor doctor, IFormFile? img)
        {
            var existingDoctor = await _context.Doctors
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Id == doctor.Id);
            if (existingDoctor == null)
            {
                return View("NotFound");
            }

            if (img == null || img?.Length == 0)
            {
                doctor.Img = existingDoctor.Img;
            }
            else
            {
                var ext = Path.GetExtension(img.FileName).ToLowerInvariant();
                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await img.CopyToAsync(stream);
                    doctor.Img = fileName;
                }

                if (!string.IsNullOrEmpty(existingDoctor.Img))
                {
                    var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingDoctor.Img);

                    if (Path.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }
            }

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return View("NotFound");
            }

            if (!string.IsNullOrEmpty(doctor.Img))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", doctor.Img);

                if (Path.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
