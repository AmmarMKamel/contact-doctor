namespace ContactDoctor.DTOs;

public class CreateAppointment
{
    public required string PatientName { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentTime { get; set; }
    public int DoctorId { get; set; }
}