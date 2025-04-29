using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Admin
{
	public class AdminController : BaseApiController
	{
		private readonly IAdminService _adminService;

		public AdminController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginDto loginDto)
		{
			var token = await _adminService.LoginAdminAsync(loginDto);

			return Ok(new { Data = token });
		}
	}
}
