using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NeuroTumAI.Service.Dtos.Account
{
	public class PatientRegisterDto
	{
		public IFormFile? ProfilePicture { get; set; }

		[Required(ErrorMessage = "fullName_required")]
		[MaxLength(50, ErrorMessage = "fullName_maxLength")]
		[RegularExpression(@"^[\p{L}]+ [\p{L}]+$", ErrorMessage = "fullName_twoWords")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "username_required")]
		[MinLength(3, ErrorMessage = "username_minLength")]
		[MaxLength(20, ErrorMessage = "username_maxLength")]
		public string Username { get; set; } = string.Empty;

		[Required(ErrorMessage = "gender_required")]
		[RegularExpression("^(Male|Female)$", ErrorMessage = "gender_invalid")]
		public string Gender { get; set; } = string.Empty;

		[Required(ErrorMessage = "email_required")]
		[EmailAddress(ErrorMessage = "email_invalid")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "dob_required")]
		[DataType(DataType.Date, ErrorMessage = "dob_invalid")]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "password_required")]
		[MinLength(6, ErrorMessage = "password_minLength")]
		[RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-]).{6,}$",
			ErrorMessage = "password_complexity")]
		public string Password { get; set; } = string.Empty;

		[Range(-90, 90, ErrorMessage = "latitude_range")]
		public decimal Latitude { get; set; }

		[Range(-180, 180, ErrorMessage = "longitude_range")]
		public decimal Longitude { get; set; }
	}
}
