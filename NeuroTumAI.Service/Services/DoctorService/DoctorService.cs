using NeuroTumAI.Core;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.Service.Services.DoctorService
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IClinicService _clinicService;
		private readonly ILocalizationService _localizationService;

		public DoctorService(IUnitOfWork unitOfWork, IClinicService clinicService, ILocalizationService localizationService)
		{
			_unitOfWork = unitOfWork;
			_clinicService = clinicService;
			_localizationService = localizationService;
		}
		public async Task<Doctor> GetDoctorByClinicIdAsync(int clinicId)
		{
			var clinic = await _clinicService.GetClinicByIdAsync(clinicId);
			if (clinic is null || !clinic.IsApproved)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("ClinicNotFound"));

			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(clinic.DoctorId);
			var doctor = await doctorRepo.GetWithSpecAsync(doctorSpec);

			return doctor!;
		}

		public async Task<Doctor> GetDoctorByUserIdAsync(string userId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpec = new DoctorSpecifications(userId);
			return await doctorRepo.GetWithSpecAsync(doctorSpec);
		}

		public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			return await doctorRepo.GetAsync(doctorId);
		}

		public async Task<IReadOnlyList<Doctor>> GetPendingDoctorsAsync(PendingDoctorSpecParams model)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpecs = new PendingDoctorSpecifications(model);

			return await doctorRepo.GetAllWithSpecAsync(doctorSpecs);
		}

		public async Task<int> GetPendingDoctorsCountAsync(PendingDoctorSpecParams model)
		{
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var doctorSpecs = new PendingDoctorCountSpecifications(model);

			return await doctorRepo.GetCountAsync(doctorSpecs);
		}
	}
}
