using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities
{
	public class Clinic : BaseEntity
	{
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string LicenseDocument { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
        public bool IsApproved { get; set; } = false;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
