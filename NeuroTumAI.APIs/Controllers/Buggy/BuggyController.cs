using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Errors;

namespace NeuroTumAI.APIs.Buggy
{
    [ApiController]
    [Route("[controller]")]
    public class BuggyController : ControllerBase
    {
		[HttpGet("notfound")] //Get: /buggy/notfound
		public IActionResult GetNotFoundRequest()
		{
			//throw new NotFoundException();
			return NotFound(new ApiResponse(404)); //404

		}

		[HttpGet("servererror")] //Get:/buggy/servererror
		public IActionResult GetServerError()
		{
			
			throw new Exception();//500
		}


		[HttpGet("badrequest")] //Get:/buggy/badrequest
		public IActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400)); //400
		}

		[HttpGet("badrequest/{id}")] //Get: /buggy/badrequest/five
		public IActionResult GetValidationError(int id )
		{
			
			return Ok(); //400
		}


		[HttpGet("unautherized")] //Get: /buggy/unautherized
		public IActionResult GetUnautherizedError()
		{
			return Unauthorized(new ApiResponse(401)); //401
		}

		[HttpGet("forbidden")] //Get: /buggy/forbidden
		public IActionResult GetForbiddenRequest()
		{
			return Forbid();
		}
		[Authorize]
		[HttpGet("autherized")] //Get: /buggy/autherized
		public IActionResult GetAutherizedRequest()
		{
			return Ok(); //400
		}

	}
}
