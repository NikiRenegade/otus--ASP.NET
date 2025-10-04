using System;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
namespace PromoCodeFactory.DataAccess.Configurations;

public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PromoCode> builder)
	{
		builder.ToTable("PromoCodes");
		builder.HasKey(pc => pc.Id);
		builder.Property(pc => pc.Code).IsRequired().HasMaxLength(50);
		builder.Property(pc => pc.ServiceInfo).HasMaxLength(200);
		builder.Property(pc => pc.BeginDate).IsRequired();
		builder.Property(pc => pc.EndDate).IsRequired();
		builder.Property(pc => pc.PartnerName).IsRequired().HasMaxLength(100);
		builder
			.HasOne(pc => pc.PartnerManager)
			.WithMany()
			.HasForeignKey(x => x.PartnerManagerId)
			.IsRequired();
		builder
			.HasOne(pc => pc.Customer)
			.WithMany(c => c.PromoCodes)
			.HasForeignKey(x => x.CustomerId)
			.IsRequired();
		builder
			.HasOne(pc => pc.Preference)
			.WithMany()
			.HasForeignKey(x => x.PreferenceId)
			.IsRequired();
	}
}
