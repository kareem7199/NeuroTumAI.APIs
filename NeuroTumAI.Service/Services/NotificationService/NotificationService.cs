using Microsoft.Extensions.Logging;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Entities.Notification;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.PatientSpecs;

namespace NeuroTumAI.Service.Services.NotificationService
{
	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFireBaseNotificationService _fireBaseNotificationService;
		private readonly ILogger<NotificationService> _logger;

		public NotificationService(IUnitOfWork unitOfWork, IFireBaseNotificationService fireBaseNotificationService, ILogger<NotificationService> logger)
		{
			_unitOfWork = unitOfWork;
			_fireBaseNotificationService = fireBaseNotificationService;
			_logger = logger;
		}

		public async Task SendAppointmentCancellationNotificationsAsync(List<AppointmentCancellationNotificationDto> notifications)
		{
			var patientIds = notifications.Select(N => N.PatientId);

			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpecs = new PatientSpecifications(patientIds);

			var patients = await patientRepo.GetAllWithSpecAsync(patientSpecs);

			foreach (var notification in notifications)
			{
				var tokens = patients.Where(P => P.Id == notification.PatientId).FirstOrDefault()!.ApplicationUser.DeviceTokens;

				string title = "Your Appointment has been Cancelled";
				string body = $"We regret to inform you that your appointment scheduled for {notification.Date:MMMM dd, yyyy} has been cancelled.";

				if (tokens.Any())
				{
					foreach (var token in tokens)
					{
						_fireBaseNotificationService.SendNotificationAsync(title, body, token.FcmToken, NotificationType.AppointmentCancellation);
					}
				}
			}
		}
	}
}
