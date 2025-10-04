using System;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
namespace PromoCodeFactory.DataAccess.Configurations;

public class PreferenceConfigurations : IEntityTypeConfiguration<Preference>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Preference> builder)
	{
		builder.ToTable("Preferences");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
		builder
			.HasMany(x => x.CustomerPreferences)
			.WithOne(x => x.Preference)
			.HasForeignKey(x => x.PreferenceId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasData(FakeDataFactory.Preferences);
	}
}
