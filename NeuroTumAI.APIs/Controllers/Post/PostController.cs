using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Pagination;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Services.Contract;

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

			var post = await _postService.AddPostAsync(model, userId!);

			return Ok(new { Message = post.Id });
		}

		[HttpPost("toggleLike/{postId}")]
		public async Task<ActionResult<ToggleLikeResponseDto>> Togglike(int postId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var result = await _postService.ToggleLikeAsync(userId, postId);

			return Ok(result);
		}

		[HttpPost("comment/{postId}")]
		public async Task<ActionResult> AddComment(int postId, AddCommentDto commentDto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var comment = await _postService.AddCommentAsync(userId, commentDto, postId);

			return Ok(new { Message = comment.Id });
		}

		[HttpGet]
		public async Task<ActionResult<CursorPaginationDto<PostToReturnDto>>> GetPosts([FromQuery] int cursor)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var posts = await _postService.GetPostsAsync(userId, cursor);

			var lastPost = posts.LastOrDefault();
			var nextCursor = lastPost?.Id ?? 0;

			var cursorPaginationDto = new CursorPaginationDto<PostToReturnDto>(nextCursor, posts);

			return Ok(cursorPaginationDto);
		}
	}
}
