using Microsoft.AspNetCore.Identity;

namespace NeuroTumAI.Core.Identity
{
	public class ApplicationUser : IdentityUser
	{
        public string? ProfilePicture { get; set; }
        public string FullName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public Gender Gender { get; set; }

	}
}
