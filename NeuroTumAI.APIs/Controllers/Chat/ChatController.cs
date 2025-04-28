using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Chat;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.APIs.Controllers.Chat
{
	[Authorize]
	public class ChatController : BaseApiController
	{
		private readonly IChatService _chatService;
		private readonly IMapper _mapper;

		public ChatController(IChatService chatService, IMapper mapper)
		{
			_chatService = chatService;
			_mapper = mapper;
		}

		[HttpPost("sendMessage")]
		public async Task<ActionResult<MessageToReturnDto>> SendMessage([FromBody] SendMessageDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var message = await _chatService.SendMessageAsync(dto, userId);

			return Ok(_mapper.Map<MessageToReturnDto>(message));
		}

	}
}
