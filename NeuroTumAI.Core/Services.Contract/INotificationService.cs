using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Notification;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface INotificationService
	{
		Task SendAppointmentCancellationNotificationsAsync(List<AppointmentCancellationNotificationDto> notifications);
		Task SendAppointmentTimeChangeNotificationsAsync(List<AppointmentTimeChangeNotificationDto> notifications, TimeOnly time);
	}
}
