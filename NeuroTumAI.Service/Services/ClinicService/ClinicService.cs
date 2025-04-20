using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.DoctorSpecs;
using NeuroTumAI.Core.Specifications.SlotSpecs;

namespace NeuroTumAI.Service.Services.ClinicService
{
	public class ClinicService : IClinicService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILocalizationService _localizationService;
		private readonly IBlobStorageService _blobStorageService;

		public ClinicService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IBlobStorageService blobStorageService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizationService = localizationService;
			_blobStorageService = blobStorageService;
		}

		public async Task<Clinic> AddClinicAsync(BaseAddClinicDto model, string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			using var clinicLicenseDocumentStream = model.LicenseDocument.OpenReadStream();
			var clinicLicenseDocument = await _blobStorageService.UploadFileAsync(clinicLicenseDocumentStream, model.LicenseDocument.FileName, "clinic-licenses");

			var location = new NetTopologySuite.Geometries.Point(model.Longitude, model.Latitude) { SRID = 4326 };

			var newClinic = new Clinic()
			{
				Address = model.Address,
				Location = location,
				PhoneNumber = model.PhoneNumber,
				DoctorId = doctor.Id,
				LicenseDocument = clinicLicenseDocument
			};

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			clinicRepo.Add(newClinic);

			await _unitOfWork.CompleteAsync();

			return newClinic;
		}

		public async Task<Slot> AddSlotAsync(AddSlotDto slot, string userId)
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

		public async Task<IReadOnlyList<Clinic>> GetClinicsAsync(ClinicSpecParams model)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpec = new NearbyClinicsSpecifications(model);
			var clinics = await clinicRepo.GetAllWithSpecAsync(clinicSpec);

			return clinics;
		}

		public async Task<IReadOnlyList<Slot>> GetClinicSlotsAsync(string userId, int clinicId, DayOfWeek day)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinic = await clinicRepo.GetAsync(clinicId);

			if (clinic is null || clinic.DoctorId != doctor.Id)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var slotaRepo = _unitOfWork.Repository<Slot>();
			var slotSpecs = new SlotSpecifications(day, clinicId);
			var slots = await slotaRepo.GetAllWithSpecAsync(slotSpecs);

			return slots;
		}

		public async Task<int> GetCountAsync(ClinicSpecParams specParams)
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpec = new NearbyClinicsForCountSpecifications(specParams);

			var count = await clinicRepo.GetCountAsync(clinicSpec);

			return count;
		}

		public async Task<IReadOnlyList<Clinic>> GetDoctorClinicAsync(string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var clinicSpecs = new ClinicSpecifications(doctor.Id);

			var clinics = await clinicRepo.GetAllWithSpecAsync(clinicSpecs);

			return clinics;
		}
	}
}
