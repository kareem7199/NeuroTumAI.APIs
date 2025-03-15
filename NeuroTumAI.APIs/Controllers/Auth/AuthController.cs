using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.APIs.Controllers.Auth
{
	public class AuthController : BaseApiController
	{
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;

		public AuthController(IMapper mapper, IAccountService accountService)
		{
			_mapper = mapper;
			_accountService = accountService;
		}

		[HttpPost("register/patient")]
		public async Task<ActionResult<RegisterResponseDto>> RegisterPatient(PatientRegisterDto model)
		{
			var newPatient = await _accountService.RegisterPatientAsync(model);

			return Ok(new RegisterResponseDto() { Email = newPatient.ApplicationUser.Email!});
		}

		[HttpPost("verifyEmail")]
		public async Task<ActionResult> VerifyEmail(VerifyEmailDto model)
		{
			await _accountService.VerifyEmailAsync(model);

			return Ok();
		}
	}
}
