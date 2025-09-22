using System;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
	Task<Customer> GetCustomerWithPreferenceByIdAsync(Guid id);
}
