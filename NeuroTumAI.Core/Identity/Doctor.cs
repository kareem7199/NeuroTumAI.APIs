using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Identity
{
	public class Doctor : BaseEntity
	{
		public bool IsApproved { get; set; } = false;
        public string LicenseDocumentFront { get; set; }
        public string LicenseDocumentBack { get; set; }
		public ICollection<Clinic> Clinics { get; set; } = new HashSet<Clinic>();
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}
