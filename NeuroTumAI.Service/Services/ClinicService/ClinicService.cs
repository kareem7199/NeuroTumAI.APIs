using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.DoctorSpecs;
using NeuroTumAI.Core.Specifications.SlotSpecs;

namespace NeuroTumAI.Service.Services.ClinicService
{
	public class ClinicService : IClinicService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILocalizationService _localizationService;

		public ClinicService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizationService = localizationService;
		}
		public async Task<Slot> AddSlot(AddSlotDto slot, string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var newSlot = _mapper.Map<Slot>(slot);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(newSlot.ClinicId);

			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var slotRepo = _unitOfWork.Repository<Slot>();
			var slotSpecs = new SlotSpecifications(newSlot.DayOfWeek, newSlot.StartTime, clinic.Id);
			var existedSlot = await slotRepo.GetWithSpecAsync(slotSpecs);

			if (existedSlot is not null)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("SlotAlreadyExists"));

			slotRepo.Add(newSlot);

			await _unitOfWork.CompleteAsync();

			return newSlot;
		}
	}
}
