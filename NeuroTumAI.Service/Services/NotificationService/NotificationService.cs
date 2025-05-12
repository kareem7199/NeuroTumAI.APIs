using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.Service.Services.NotificationService
{
	public class NotificationService : INotificationService
	{
		private static bool _isInitialized;

		public NotificationService()
		{
			if (!_isInitialized)
			{
				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromFile("../NeuroTumAI.Service/Services/NotificationService/nuerotum-firebase-adminsdk-fbsvc-927b811ac6.json")
				});
				_isInitialized = true;
			}
		}
		public async Task SendNotificationAsync(string title, string body, string fcmToken)
		{
			var message = new Message()
			{
				Token = fcmToken,
				Notification = new Notification
				{
					Title = title,
					Body = body,
				}
			};

			await FirebaseMessaging.DefaultInstance.SendAsync(message);
		}
	}
}
