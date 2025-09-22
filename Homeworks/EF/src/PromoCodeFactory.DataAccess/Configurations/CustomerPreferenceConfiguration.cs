using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
namespace PromoCodeFactory.DataAccess.Configurations;

public class CustomerPreferenceConfiguration : IEntityTypeConfiguration<CustomerPreference>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CustomerPreference> builder)
	{
		builder.ToTable("CustomersPreferences");
		builder.HasKey(x => new { x.CustomerId, x.PreferenceId });
		builder
			.HasOne(x => x.Customer)
			.WithMany(x => x.CustomerPreferences)
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
		builder
			.HasOne(x => x.Preference)
			.WithMany(x => x.CustomerPreferences)
			.HasForeignKey(x => x.PreferenceId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasData(FakeDataFactory.CustomersPreferences);
	}
}
