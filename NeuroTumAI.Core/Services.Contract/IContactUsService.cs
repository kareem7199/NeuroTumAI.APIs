using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Contact_Us;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
    public interface IContactUsService
    {
        Task <ContactUS> SendMessageAsync(ContactUsDto model, string userId);

    }
}
