namespace ContactDoctor.DTOs;

public class CreateAppointment
{
    public required string PatientName { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public int DoctorId { get; set; }
}