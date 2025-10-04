using System;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.Core.Domain.Administration;
using System.Linq;
namespace PromoCodeFactory.WebHost.Mappers;

public static class EmployeeMapper
{
	public static EmployeeResponse ToEmployeeResponse(this Employee employee)
	{
		if (employee == null) throw new ArgumentNullException(nameof(employee));

		return new EmployeeResponse
		{
			Id = employee.Id,
			Email = employee.Email,
			Roles = employee.Roles.Select(x => new RoleItemResponse()
			{
				Name = x.Name,
				Description = x.Description
			}).ToList(),
			FullName = employee.FullName,
			AppliedPromocodesCount = employee.AppliedPromocodesCount
		};
	}
	public static Employee ToEmployee(this EmployeeRequest request)
	{
		if (request == null) throw new ArgumentNullException(nameof(request));

		return new Employee
		{
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email,
			Roles = request.Roles.Select(x => new Role()
			{
				Name = x.Name,
				Description = x.Description
			}).ToList(),
			AppliedPromocodesCount = request.AppliedPromocodesCount
		};
	}

}
