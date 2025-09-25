// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const datePicker = flatpickr("#appointment-date", {
    minDate: "today",
    dateFormat: "Y-m-d",
    disable: [function (date) {
        return date.getDay() === 5 || date.getDay() === 6;
    }]
});

// Time Picker
const timePicker = flatpickr("#appointment-time", {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    time_24hr: true,
    minuteIncrement: 30,
    minTime: "09:00",
    maxTime: "21:00"
});

const patientNameInput = document.getElementById("patient-name");
const doctorId = document.getElementById("doctor-id").value;

// Form validation
const form = document.querySelector('form');
form.addEventListener('submit', async function (e) {
    if (!datePicker.input.value || !timePicker.input.value || !patientNameInput.value) {
        e.preventDefault();

        if (!datePicker.input.value) {
            datePicker.input.classList.add('is-invalid');
        }
        if (!timePicker.input.value) {
            timePicker.input.classList.add('is-invalid');
        }
        if (!patientNameInput.value) {
            patientNameInput.classList.add('is-invalid');
        }
    }

    e.preventDefault();

    const appointment = {
        patientName: patientNameInput.value,
        appointmentTime: timePicker.input.value,
        appointmentDate: datePicker.input.value,
        doctorId
    };
    
    const response = await fetch("/Doctors/CompleteAppointmentBooking", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(appointment)
    });
    if (response.ok) {
        Swal.fire({
            title: "Success!",
            text: "You booked the appointment!",
            icon: "success"
        });
    } else {
        Swal.fire({
            title: "Failed!",
            text: "This appointment is already booked!",
            icon: "error"
        });
    }
});

// Remove invalid class when user starts typing
[datePicker.input, timePicker.input, patientNameInput].forEach(input => {
    input.addEventListener('change', function () {
        this.classList.remove('is-invalid');
    });
});