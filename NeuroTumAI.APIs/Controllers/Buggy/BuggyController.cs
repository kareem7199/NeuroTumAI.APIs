using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Exceptions;


namespace NeuroTumAI.APIs.Buggy
{

	public class BuggyController : BaseApiController
	{
		[HttpGet("notfound")] //Get: api/buggy/notfound
		public IActionResult GetNotFoundRequest()
		{
			throw new NotFoundException("Not Found Exception");

		}

		[HttpGet("servererror")] //Get:api/buggy/servererror
		public IActionResult GetServerError()
		{
			throw new Exception("Internal Server Error");//500
		}


		[HttpGet("badrequest")] //Get:api/buggy/badrequest
		public IActionResult GetBadRequest()
		{
			throw new BadRequestException("Bad Request Exception");
		}

		[HttpGet("validationerror")]
		public IActionResult PostValidationError()
		{
			throw new ValidationException("Validation error")
			{
				Errors = ["Id is required.", "Id must be at least 1."]
			};
		}
	}
}
