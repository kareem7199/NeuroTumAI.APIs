using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Clinic
{
	public class ClinicController : BaseApiController
	{
		private readonly IClinicService _clinicService;

		public ClinicController(IClinicService clinicService)
        {
			_clinicService = clinicService;
		}

		[Authorize(Roles = "Doctor" , Policy = "ActiveUserOnly")]
		[HttpPost("slot")]
		public async Task<ActionResult> AddSlot([FromBody] AddSlotDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			
			var slot = await _clinicService.AddSlot(model , userId);

			return Ok(slot);
		}
	}
}
