using AutoMapper;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.Service.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<PatientRegisterDto, Patient>();
		}
	}
}
