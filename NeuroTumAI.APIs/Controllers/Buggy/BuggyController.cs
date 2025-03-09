﻿using LinkDev.Talabat.APIs.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Errors;

namespace NeuroTumAI.APIs.Buggy
{
   
    public class BuggyController : BaseApiController
    {
		[HttpGet("notfound")] //Get: api/buggy/notfound
		public IActionResult GetNotFoundRequest()
		{
			//throw new NotFoundException();
			return NotFound(new ApiResponse(404)); //404

		}

		[HttpGet("servererror")] //Get:api/buggy/servererror
		public IActionResult GetServerError()
		{
			
			throw new Exception();//500
		}


		[HttpGet("badrequest")] //Get:api/buggy/badrequest
		public IActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400)); //400
		}

		[HttpGet("badrequest/{id}")] //Get: api/buggy/badrequest/five
		public IActionResult GetValidationError(int id )
		{
			
			return Ok(); //400
		}


		[HttpGet("unautherized")] //Get: api/buggy/unautherized
		public IActionResult GetUnautherizedError()
		{
			return Unauthorized(new ApiResponse(401)); //401
		}

		[HttpGet("forbidden")] //Get: api/buggy/forbidden
		public IActionResult GetForbiddenRequest()
		{
			return Forbid();
		}
		[Authorize]
		[HttpGet("autherized")] //Get: api/buggy/autherized
		public IActionResult GetAutherizedRequest()
		{
			return Ok(); //400
		}

	}
}
