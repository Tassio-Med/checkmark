using Microsoft.EntityFrameworkCore;
using Checkmark.API.Data;
using Checkmark.API.Interfaces;
using Checkmark.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Checkmark.API.Repositories
{
  public class CheckmarkRepository : ICheckmarkRepository
  {
    private readonly DataDbContext _context;
    public CheckmarkRepository(DataDbContext context)
    {
      _context = context;
    }

    public async Task<CheckmarkItem?> GetByIdAsync(int id)
    {
      return await _context.CheckmarkItems.FindAsync(id);
    }

    public async Task<IEnumerable<CheckmarkItem>> GetAllAsync()
    {
      return await _context.CheckmarkItems.ToListAsync();
    }

    public async Task AddAsync(CheckmarkItem entity)
    {
      await _context.CheckmarkItems.AddAsync(entity);
      await _context.SaveChangesAsync();
    }

    public async Task<CheckmarkItem?> UpdateAsync(CheckmarkItem entity)
    {
      entity.UpdatedAt = DateTime.UtcNow;
      _context.CheckmarkItems.Update(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var entity = await GetByIdAsync(id);
      if (entity != null)
      {
        _context.CheckmarkItems.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
      }
      return false;
    }

    public async Task<bool> ExistsAsync(int id)
    {
      return await _context.CheckmarkItems.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<CheckmarkItem>> GetCompletedItemsAsync()
    {
      return await _context.CheckmarkItems
        .Where(i => i.IsCompleted)
        .ToListAsync();
    }

    public async Task<IEnumerable<CheckmarkItem>> GetPendingItemsAsync()
    {
      return await _context.CheckmarkItems
        .Where(i => !i.IsCompleted)
        .ToListAsync();
    }

    public async Task<IEnumerable<CheckmarkItem>> GetItemsByPriorityAsync(PriorityLevel priority)
    {
      return await _context.CheckmarkItems
        .Where(i => i.Priority == priority)
        .ToListAsync();
    }
  }
}