using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IAuthService
	{
		Task<string> CreateTokenAsync(ApplicationUser user);
	}
}
