using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Specifications.ClinicSpecs;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IClinicService
	{
		Task<Clinic?> GetClinicByIdAsync(int clinicId);
		Task<IReadOnlyList<Clinic>> GetDoctorClinicAsync(string userId);
		Task<IReadOnlyList<Clinic>> GetClinicsAsync(ClinicSpecParams specParams);
		Task<int> GetCountAsync(ClinicSpecParams specParams);
		Task<IReadOnlyList<Slot>> GetClinicSlotsAsync(string userId, int clinicId, DayOfWeek day);
		Task<Clinic> AddClinicAsync(BaseAddClinicDto model, string userId);
		Task<Slot> AddSlotAsync(AddSlotDto slot, string userId);
	}
}
