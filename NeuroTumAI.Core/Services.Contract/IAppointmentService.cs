using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IAppointmentService
	{
		Task<Appointment> BookAppointmentAsync(BookAppointmentDto model, string userId);
		Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(string userId, int clinicId, DateOnly date);
	}
}
