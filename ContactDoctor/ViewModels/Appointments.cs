using ContactDoctor.Models;

namespace ContactDoctor.ViewModels;

public class Appointments
{
    public ICollection<Appointment> AppointmentsList { get; set; } = new List<Appointment>();
}