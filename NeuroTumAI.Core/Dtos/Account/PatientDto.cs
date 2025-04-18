using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class PatientDto : UserDto
	{
        public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
	}
}
