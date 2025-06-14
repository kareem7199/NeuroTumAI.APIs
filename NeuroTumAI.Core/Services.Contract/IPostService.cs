using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IPostService
	{
		Task<Post> AddPostAsync(AddPostDto model, string applicationUserId);
		Task<Comment> AddCommentAsync(string userId, AddCommentDto model, int postId);
		Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId);
	}
}
