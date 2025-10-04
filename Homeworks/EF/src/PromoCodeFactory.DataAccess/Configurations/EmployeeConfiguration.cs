using System;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
	{
		builder.ToTable("Employees");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.FirstName).IsRequired().HasMaxLength(30);
		builder.Property(x => x.LastName).IsRequired().HasMaxLength(30);
		builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
		builder.Property(x => x.AppliedPromocodesCount).HasDefaultValue(0);
		builder.HasOne(x => x.Role)
			.WithMany()
			.HasForeignKey(x => x.RoleId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasData(FakeDataFactory.Employees);
	}

}
