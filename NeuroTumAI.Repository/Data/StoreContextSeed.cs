using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Repository.Data
{
	public static class StoreContextSeed
	{
		private static string[] roles = { "Patient", "Doctor" };
		private static string[] reviews = { "Pretty sure he got his degree from YouTube tutorials.", "he tried… i think. but so does my cat when it walks on the keyboard.", "Not bad, not great. Like microwave food – gets the job done, barely.", "Pretty decent! He knew what he was doing… most of the time.", "Best doctor ever." };
		private static string[] times =
		{
			"09:00:00",
			"10:00:00",
			"11:00:00",
			"12:00:00",
			"13:00:00",
			"14:00:00",
			"15:00:00",
			"16:00:00",
			"17:00:00",
			"18:00:00"
		};
		public async static Task SeedAsync(StoreContext _dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			if (_dbContext.UserRoles.Any()) return;
			await SeedRolesAsync(roleManager);
			await SeedPatientsAsync(userManager, _dbContext);
			await SeedDoctorsAsync(userManager, _dbContext);
			await _dbContext.SaveChangesAsync();
			await SeedAppointmentsWithReviewsAsync(_dbContext);
			await _dbContext.SaveChangesAsync();
		}
		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			foreach (var roleName in roles)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
		}
		private static async Task SeedPatientsAsync(UserManager<ApplicationUser> userManager, StoreContext _dbContext)
		{
			for (int i = 0; i < 10; i++)
			{
				var newAccount = new ApplicationUser()
				{
					FullName = $"Patient{i + 1}",
					Email = $"patient{i + 1}@gmail.com",
					UserName = $"Patient{i + 1}",
					Gender = Gender.Male,
					DateOfBirth = DateTime.Parse("2003-04-21"),
					EmailConfirmed = true
				};
				await userManager.CreateAsync(newAccount, "Pa$$w0rd");
				await userManager.AddToRoleAsync(newAccount, "Patient");
				var newPatient = new Patient()
				{
					ApplicationUserId = newAccount.Id,
					Latitude = 90,
					Longitude = 90
				};
				await _dbContext.AddAsync(newPatient);
			}
		}
		private static async Task SeedDoctorsAsync(UserManager<ApplicationUser> userManager, StoreContext _dbContext)
		{
			for (int i = 0; i < 5; i++)
			{
				var newAccount = new ApplicationUser()
				{
					ProfilePicture = "",
					FullName = $"Doctor{i + 1}",
					Email = $"doctor{i + 1}@gmail.com",
					UserName = $"Doctor{i + 1}",
					Gender = Gender.Male,
					DateOfBirth = DateTime.Parse("2003-04-21"),
					EmailConfirmed = true
				};
				await userManager.CreateAsync(newAccount, "Pa$$w0rd");
				await userManager.AddToRoleAsync(newAccount, "Doctor");
				var newDoctor = new Doctor()
				{
					ApplicationUserId = newAccount.Id,
					LicenseDocumentBack = "",
					LicenseDocumentFront = "",
					IsApproved = true
				};
				var location = new NetTopologySuite.Geometries.Point(31.2357, 30.0444) { SRID = 4326 };
				var newClinic = new Clinic()
				{
					Address = "123 Health Street, Cairo, Egypt",
					Location = location,
					PhoneNumber = "+20 100 123 4567",
					LicenseDocument = "dummy-license.pdf",
					IsApproved = true
				};

				for (int day = 0; day < 7; ++day)
				{
					foreach (var time in times)
					{
						var slot = new Slot()
						{
							StartTime = TimeOnly.Parse(time),
							DayOfWeek = (DayOfWeek)day
						};
						newClinic.Slots.Add(slot);
					}
				}

				newDoctor.Clinics.Add(newClinic);
				await _dbContext.AddAsync(newDoctor);
			}
		}
		private static async Task SeedAppointmentsWithReviewsAsync(StoreContext _dbContext)
		{
			for (int patientId = 1; patientId <= 10; patientId++)
			{
				for (int doctorId = 1; doctorId <= 5; doctorId++)
				{
					var newAppointment = new Appointment()
					{
						DoctorId = doctorId,
						PatientId = patientId,
						ClinicId = doctorId,
						Date = DateOnly.Parse("2025-04-25"),
						StartTime = TimeOnly.Parse(times[patientId - 1]),
						Status = AppointmentStatus.Completed
					};
					await _dbContext.AddAsync(newAppointment);

					int stars = Random.Shared.Next(1, 6);
					var newReview = new Review()
					{
						DoctorId = doctorId,
						PatientId = patientId,
						Stars = stars,
						Comment = reviews[stars - 1]
					};

					await _dbContext.AddAsync(newReview);
				}

			}
		}
	}
}
