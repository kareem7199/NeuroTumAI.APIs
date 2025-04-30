using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Admin;
using NeuroTumAI.Core.Dtos.Doctor;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.APIs.Controllers.Admin
{
	public class AdminController : BaseApiController
	{
		private readonly IAdminService _adminService;
		private readonly IMapper _mapper;
		private readonly IDoctorService _doctorService;

		public AdminController(IAdminService adminService, IMapper mapper, IDoctorService doctorService)
		{
			_adminService = adminService;
			_mapper = mapper;
			_doctorService = doctorService;
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

		[Authorize(Roles = "Admin")]
		[HttpGet("pendingDoctors")]
		public async Task<ActionResult> GetPendingDoctors([FromQuery] PendingDoctorSpecParams specParams)
		{
			var doctors = await _doctorService.GetPendingDoctorsAsync(specParams);
			var count = await _doctorService.GetPendingDoctorsCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<PendingDoctorDto>>(doctors);

			return Ok(new PaginationDto<PendingDoctorDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("pendingDoctors/accept/{doctorId}")]
		public async Task<ActionResult> AcceptPendingDoctor(int doctorId)
		{
			await _doctorService.AcceptPendingDoctorAsync(doctorId);

			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("pendingDoctors/reject/{doctorId}")]
		public async Task<ActionResult> RejectPendingDoctor(int doctorId)
		{
			await _doctorService.RejectPendingDoctorAsync(doctorId);

			return Ok();
		}
	}
}
