using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.ContactUs
{
    public class ContactUsDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter You Message")]
        [StringLength(1000, ErrorMessage = "Message Length")]
        public string Message { get; set; }


    }
}
