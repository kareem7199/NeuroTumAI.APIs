using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NeuroTumAI.Repository.Data
{
	public static class StoreContextSeed
	{
		private static string[] roles = { "Patient", "Doctor" };
		public async static Task SeedAsync(StoreContext _dbContext , RoleManager<IdentityRole> roleManager)
		{
			await SeedRolesAsync(roleManager);
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
	}
}
