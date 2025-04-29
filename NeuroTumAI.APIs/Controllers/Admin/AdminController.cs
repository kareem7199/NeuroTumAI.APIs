using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Admin;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Admin
{
	public class AdminController : BaseApiController
	{
		private readonly IAdminService _adminService;
		private readonly IMapper _mapper;

		public AdminController(IAdminService adminService, IMapper mapper)
		{
			_adminService = adminService;
			_mapper = mapper;
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginDto loginDto)
		{
			var token = await _adminService.LoginAdminAsync(loginDto);

			return Ok(new { Data = token });
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult> GetAdmin()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var intUserId = int.Parse(userId);

			var admin = await _adminService.GetAdminByIdAsync(intUserId);
			if (admin is null)
				throw new UnAuthorizedException("User not found.");


			return Ok(new { Data = _mapper.Map<AdminToReturnDto>(admin) });
		}
	}
}
