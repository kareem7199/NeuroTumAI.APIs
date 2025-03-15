using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Identity
{
	public class Doctor : BaseEntity
	{
		public bool IsApproved { get; set; } = false;
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}
