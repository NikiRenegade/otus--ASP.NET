using System;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.EntityFramework.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
	{
		builder.ToTable("Roles");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
		builder.Property(x => x.Description).HasMaxLength(200);
		builder.HasData();
	}
}
