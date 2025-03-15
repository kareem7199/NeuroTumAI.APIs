using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Identity
{
	public class Patient : BaseEntity
	{
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}
