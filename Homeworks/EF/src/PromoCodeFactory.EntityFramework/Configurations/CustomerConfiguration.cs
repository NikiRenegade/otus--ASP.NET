using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.EntityFramework.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> builder)
	{
		builder.ToTable("Customers");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.FirstName).IsRequired().HasMaxLength(30);
		builder.Property(x => x.LastName).IsRequired().HasMaxLength(30);
		builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
		builder
			.HasMany(x => x.CustomerPreferences)
			.WithOne(x => x.Customer)
			.HasForeignKey("CustomerId")
			.OnDelete(DeleteBehavior.Cascade);
		builder
			.HasMany(x => x.PromoCodes)
			.WithOne(x => x.Customer)
			.HasForeignKey("CustomerId")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
