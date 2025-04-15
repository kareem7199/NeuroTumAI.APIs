using Microsoft.AspNetCore.SignalR;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities.Post_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.PostSpecs.LikeSpecs;
using NeuroTumAI.Service.Hubs;

namespace NeuroTumAI.Service.Services.PostService
{
	public class PostService : IPostService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHubContext<PostHub> _hubContext;
		private readonly ILocalizationService _localizationService;

		public PostService(IUnitOfWork unitOfWork,IHubContext<PostHub> hubContext,ILocalizationService localizationService)
		{
			_unitOfWork = unitOfWork;
			_hubContext = hubContext;
			_localizationService = localizationService;
		}
		public async Task<Post> AddPostAsync(AddPostDto model, string applicationUserId)
		{
			var newPost = new Post() {
				Title = model.Title,
				Content = model.Content,
				ApplicationUserId = applicationUserId
			};

			var postRepo = _unitOfWork.Repository<Post>();

			postRepo.Add(newPost);
			await _unitOfWork.CompleteAsync();

			return newPost;
		}

		public async Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId)
		{
			var postRepo = _unitOfWork.Repository<Post>();
			var post = await postRepo.GetAsync(postId);

			if (post is null)
				throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

			var likeRepo = _unitOfWork.Repository<Like>();

			var likeSpec = new LikeByUserAndPostSpecification(userId , postId);
			var existingLike = await likeRepo.GetWithSpecAsync(likeSpec);
			if (existingLike is not null)
			{
				likeRepo.Delete(existingLike);
				post.LikesCount--;
				postRepo.Update(post);
			} else
			{
				var newLike = new Like()
				{
					ApplicationUserId = userId,
					PostId = postId
				};
				likeRepo.Add(newLike);
				post.LikesCount++;
				postRepo.Update(post);
			}

			await _unitOfWork.CompleteAsync();
			await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new {
				PostId = postId,
				post.LikesCount,
				post.CommentsCount
			});

			return new ToggleLikeResponseDto()
			{
				IsLiked = existingLike is null,
				PostId = postId
			};
		}
	}
}
