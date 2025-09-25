using ContactDoctor.Models;

namespace ContactDoctor.ViewModels;

public class BookAppointment
{
    public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    public List<string> Specializations { get; set; } = new List<string>();
    public string? Search { get; set; }
    public string? Specialization { get; set; }
    public int Page { get; set; } = 1;
    public int PageLimit { get; set; } = 3;
    public int PagesCount { get; set; }
}