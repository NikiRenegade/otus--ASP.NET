using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories;

public class EntityFrameworkRepository<T> : IRepository<T> where T : BaseEntity
{
	protected readonly ApplicationDbContext _context;
	protected readonly DbSet<T> _entitySet;
	public EntityFrameworkRepository(ApplicationDbContext context)
	{
		_context = context;
		_entitySet = _context.Set<T>();
	}

	public async Task<T> CreateAsync(T entity)
	{
		await _entitySet.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = _entitySet.Find(id);
		if (entity == null)
		{
			return await Task.FromResult(false);
		}
		_entitySet.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _entitySet.ToListAsync();
	}

	public async Task<T> GetByIdAsync(Guid id)
	{
		return await _entitySet.FindAsync(id);
	}

	public async Task<T> UpdateAsync(T entity)
	{
		_entitySet.Update(entity);
		await _context.SaveChangesAsync();
		return entity;
	}
}
