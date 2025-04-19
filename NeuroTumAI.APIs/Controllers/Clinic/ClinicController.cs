using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Clinic
{
	public class ClinicController : BaseApiController
	{
		private readonly IClinicService _clinicService;
		private readonly IMapper _mapper;

		public ClinicController(IClinicService clinicService , IMapper mapper)
		{
			_clinicService = clinicService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpGet("doctor")]
		public async Task<ActionResult<IReadOnlyList<ClinicToReturnDto>>> GetDoctorClinics()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var clinics = await _clinicService.GetDoctorClinicAsync(userId);

			return Ok(_mapper.Map<IReadOnlyList<ClinicToReturnDto>>(clinics));
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpPost]
		public async Task<ActionResult<ClinicToReturnDto>> AddClinic([FromForm] BaseAddClinicDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var clinic = await _clinicService.AddClinic(model, userId);

			return Ok(_mapper.Map<ClinicToReturnDto>(clinic));
		}

		[Authorize(Roles = "Doctor", Policy = "ActiveUserOnly")]
		[HttpPost("slot")]
		public async Task<ActionResult> AddSlot([FromBody] AddSlotDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var slot = await _clinicService.AddSlot(model, userId);

			return Ok(new
			{
				Id = slot.Id
			});
		}
	}
}
