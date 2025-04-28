using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IDoctorService
	{
		Task<Doctor> GetDoctorByClinicIdAsync(int clinicId);
		Task<Doctor> GetDoctorByUserIdAsync(string userId);
		Task<Doctor> GetDoctorByIdAsync(int doctorId);
	}
}
