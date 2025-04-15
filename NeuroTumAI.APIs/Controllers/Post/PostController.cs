using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.APIs.Controllers.Post
{
	[Authorize]
	public class PostController : BaseApiController
	{
		private readonly IPostService _postService;

		public PostController(IPostService postService)
		{
			_postService = postService;
		}

		[HttpPost]
		public async Task<ActionResult> AddPost(AddPostDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			await _postService.AddPostAsync(model, userId!);

			return Ok();
		}

		[HttpPost("toggleLike/{postId}")]
		public async Task<ActionResult<ToggleLikeResponseDto>> Togglike(int postId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var result = await _postService.ToggleLikeAsync(userId, postId);

			return Ok(result);
		}
	}
}
