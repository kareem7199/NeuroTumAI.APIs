using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Specifications.PatientSpecs
{
	public class PatientSpecifications : BaseSpecifications<Patient>
	{
		public PatientSpecifications(string ApplicationUserId)
			: base(P => P.ApplicationUserId == ApplicationUserId)
		{
			Includes.Add(P => P.ApplicationUser);
		}
	}
}
