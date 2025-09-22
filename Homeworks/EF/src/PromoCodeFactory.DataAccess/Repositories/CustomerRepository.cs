using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
namespace PromoCodeFactory.DataAccess.Repositories;

public class CustomerRepository : EntityFrameworkRepository<Customer>, ICustomerRepository
{
	public CustomerRepository(ApplicationDbContext context) : base(context) { }

	public async Task<Customer> GetCustomerWithPreferenceByIdAsync(Guid id)
	{
		return await _context.Set<Customer>().Include(x => x.CustomerPreferences).ThenInclude(x => x.Preference).Include(x => x.PromoCodes).FirstOrDefaultAsync(x => x.Id == id);
	}
}
