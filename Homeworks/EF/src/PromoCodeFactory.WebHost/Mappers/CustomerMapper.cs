using System;
using System.Collections.Generic;
using System.Linq;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
namespace PromoCodeFactory.WebHost.Mappers;

public static class CustomerMapper
{
	public static CustomerResponse ToCustomerResponse(this Customer customer)
	{
		if (customer == null) throw new ArgumentNullException(nameof(customer));

		return new CustomerResponse
		{
			Id = customer.Id,
			FirstName = customer.FirstName,
			LastName = customer.LastName,
			Email = customer.Email,
			Preferences = customer.CustomerPreferences
						.Select(cp => new PreferenceResponse
						{
							Name = cp.Preference.Name
						}).ToList(),
			PromoCodes = customer.PromoCodes
						.Select(pc => new PromoCodeShortResponse
						{
							Id = pc.Id,
							Code = pc.Code,
							ServiceInfo = pc.ServiceInfo,
							BeginDate = pc.BeginDate.ToString("yyyy-MM-dd"),
							EndDate = pc.EndDate.ToString("yyyy-MM-dd"),
							PartnerName = pc.PartnerName

						}).ToList()
		};
	}

	public static CustomerShortResponse ToCustomerShortResponse(this Customer customer)
	{
		if (customer == null) throw new ArgumentNullException(nameof(customer));

		return new CustomerShortResponse
		{
			Id = customer.Id,
			FirstName = customer.FirstName,
			LastName = customer.LastName,
			Email = customer.Email
		};
	}

	public static Customer ToCustomer(this CreateOrEditCustomerRequest request, IEnumerable<Preference> preferences)
	{
		if (request == null) throw new ArgumentNullException(nameof(request));

		var customer = new Customer
		{
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email
		};

		customer.CustomerPreferences = preferences
				.Where(p => request.PreferenceIds.Contains(p.Id))
				.Select(p => new CustomerPreference
				{
					Customer = customer,
					Preference = p
				}).ToList();

		return customer;
	}
}
