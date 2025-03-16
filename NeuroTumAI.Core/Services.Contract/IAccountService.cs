using Microsoft.AspNetCore.Identity;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IAccountService
	{
		Task<Patient> RegisterPatientAsync(PatientRegisterDto model);
		Task<bool> VerifyEmailAsync(VerifyEmailDto model);
		Task<PatientLoginResponseDto> LoginPatientAsync(LoginDto model);
	}
}
