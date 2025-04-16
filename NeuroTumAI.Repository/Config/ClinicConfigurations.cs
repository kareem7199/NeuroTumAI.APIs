using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Repository.Config
{
	internal class ClinicConfigurations : IEntityTypeConfiguration<Clinic>
	{
		public void Configure(EntityTypeBuilder<Clinic> builder)
		{
			builder.Property(C => C.Latitude)
				   .HasColumnType("decimal(9,6)");

			builder.Property(C => C.Longitude)
				   .HasColumnType("decimal(9,6)");
		}
	}
}
