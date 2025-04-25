using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;
using NeuroTumAI.Core.Specifications.ReviewSpecs;

namespace NeuroTumAI.Service.Services.ReviewService
{
	public class ReviewService : IReviewService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILocalizationService _localizationService;

		public ReviewService(IUnitOfWork unitOfWork, ILocalizationService localizationService)
		{
			_unitOfWork = unitOfWork;
			_localizationService = localizationService;
		}
		public async Task<Review> AddReviewAsync(AddReviewDto addReviewDto, string userId)
		{
			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpec = new PatientSpecifications(userId);
			var patient = await patientRepo.GetWithSpecAsync(patientSpec);

			var appointmentRepo = _unitOfWork.Repository<Appointment>();
			var appointmentSpec = new AppointmentSpecifications(addReviewDto.DoctorId, patient.Id);
			var appointment = await appointmentRepo.GetWithSpecAsync(appointmentSpec);

			if (appointment is null)
				throw new BadRequestException(_localizationService.GetMessage<ResponsesResources>("AppointmentNotFound"));

			var reviewRepo = _unitOfWork.Repository<Review>();
			var reviewSpec = new ReviewSpecifications(patient.Id, addReviewDto.DoctorId);
			var review = await reviewRepo.GetWithSpecAsync(reviewSpec);

			if (review is null)
			{
				var newReview = new Review()
				{
					DoctorId = addReviewDto.DoctorId,
					Stars = addReviewDto.Stars,
					Comment = addReviewDto.Comment,
					PatientId = patient.Id
				};

				reviewRepo.Add(newReview);
				await _unitOfWork.CompleteAsync();

				return newReview;
			}
			else
			{
				review.Stars = addReviewDto.Stars;
				review.Comment = addReviewDto.Comment;
				review.CreatedAt = DateTime.UtcNow;

				reviewRepo.Update(review);
				await _unitOfWork.CompleteAsync();

				return review;
			}

		}

		public async Task<IReadOnlyList<Review>> GetDoctorLatest5ReviewsAsync(int doctorId)
		{
			var reviewRepo = _unitOfWork.Repository<Review>();
			var reviewSpec = new ReviewSpecifications(doctorId);
			var reviews = await reviewRepo.GetAllWithSpecAsync(reviewSpec);

			return reviews;
		}
	}
}
