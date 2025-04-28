using AutoMapper;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Dtos.Chat;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Dtos.Doctor;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Chat_Aggregate;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.Service.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<PatientRegisterDto, Patient>();
			CreateMap<Patient, PatientDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUser.Id))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture))
				.ForMember(D => D.Role, O => O.MapFrom(S => "Patient"));

			CreateMap<Doctor, UserDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUser.Id))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture))
				.ForMember(D => D.Role, O => O.MapFrom(S => "Doctor"));

			CreateMap<Clinic, ClinicWithDoctorDataDto>()
				.ForMember(D => D.DoctorProfilePicture, O => O.MapFrom(S => S.Doctor.ApplicationUser.ProfilePicture))
				.ForMember(D => D.DoctorFullName, O => O.MapFrom(S => S.Doctor.ApplicationUser.FullName))
				.ForMember(D => D.AverageStarRating, O => O.MapFrom(S => S.Doctor.Reviews.Any() ? S.Doctor.Reviews.Average(R => R.Stars) : 0));

			CreateMap<AddSlotDto, Slot>();
			CreateMap<Clinic, ClinicToReturnDto>();
			CreateMap<Slot, SlotToReturnDto>();
			CreateMap<Appointment, AppointmentToReturnDto>();
			CreateMap<Review, ReviewToReturnDto>();


			CreateMap<Patient, PublicPatientDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.Id))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture));

			CreateMap<Doctor, DoctorWithReviewsDto>()
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture))
				.ForMember(D => D.AverageStarRating, O => O.MapFrom(D => D.Reviews.Any() ? D.Reviews.Average(R => R.Stars) : 0));

			CreateMap<Appointment, AppoitntmentWithPatientDto>();

			CreateMap<ChatMessage, MessageToReturnDto>();

			CreateMap<ApplicationUser, ChatUserDto>();

			CreateMap<Conversation, ConversationToReturnDto>()
				.ForMember(D => D.User, O => O.MapFrom(S => S.FirstUser))
				.ForMember(D => D.LastMessage, O => O.MapFrom(S => S.ChatMessages.FirstOrDefault()));
;
		}
	}
}
