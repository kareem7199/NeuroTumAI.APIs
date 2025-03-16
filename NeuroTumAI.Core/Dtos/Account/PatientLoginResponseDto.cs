using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class PatientLoginResponseDto
	{
        public string Token { get; set; }
        public PatientToReturnDto User { get; set; }
    }
}
