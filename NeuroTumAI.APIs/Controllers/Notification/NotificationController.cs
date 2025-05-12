using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Notification
{
	public class NotificationController : BaseApiController
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
        {
			_notificationService = notificationService;
		}

		[HttpPost]
		public async Task<ActionResult> Send([FromQuery] string fcmToken)
		{

			await _notificationService.SendNotificationAsync("test title" , "test body" , fcmToken);
			return Ok();
		}
    }
}
