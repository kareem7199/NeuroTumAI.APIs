using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IClinicService
	{
		Task<Clinic> AddClinic(BaseAddClinicDto model, string userId);
		Task<Slot> AddSlot(AddSlotDto slot, string userId);
	}
}
