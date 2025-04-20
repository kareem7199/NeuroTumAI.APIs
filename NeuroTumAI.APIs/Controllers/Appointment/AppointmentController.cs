using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Appointment
{
	public class AppointmentController : BaseApiController
	{
		private readonly IAppointmentService _appointmentService;
		private readonly IMapper _mapper;

		public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
		{
			_appointmentService = appointmentService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Patient")]
		[HttpPost]
		public async Task<ActionResult<AppointmentToReturnDto>> BookAppointemnt([FromBody] BookAppointmentDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var appointment = await _appointmentService.BookAppointmentAsync(model, userId);

			return Ok(_mapper.Map<AppointmentToReturnDto>(appointment));
		}
	}
}
