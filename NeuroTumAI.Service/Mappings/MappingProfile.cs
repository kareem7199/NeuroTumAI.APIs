using AutoMapper;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.Service.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<PatientRegisterDto, Patient>();
			CreateMap<Patient, PatientToReturnDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUser.Id))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth));
		}
	}
}
