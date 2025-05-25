﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface INotificationService
	{
		Task SendNotificationAsync(string title, string body, string fcmToken);
	}
}
