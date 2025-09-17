using System.Collections.Generic;
using System.Threading.Tasks;
using Checkmark.API.Interfaces;
using Checkmark.API.Models;


namespace Checkmark.API.Services
{
  public class CheckmarkService : ICheckmarkService
  {
    private readonly ICheckmarkRepository _repository;
    public CheckmarkService(ICheckmarkRepository repository)
    {
      _repository = repository;
    }

    public async Task<CheckmarkItem?> GetItemByIdAsync(int id)
    {
      return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<CheckmarkItem>> GetAllItemsAsync()
    {
      return await _repository.GetAllAsync();
    }

    public async Task<CheckmarkItem> CreateItemAsync(CheckmarkItem item)
    {
      await _repository.AddAsync(item);
      return item;
    }

    public async Task<CheckmarkItem?> UpdateItemAsync(CheckmarkItem item)
    {
      return await _repository.UpdateAsync(item);
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
      return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CheckmarkItem>> GetCompletedItemsAsync()
    {
      return await _repository.GetCompletedItemsAsync();
    }

    public async Task<IEnumerable<CheckmarkItem>> GetPendingItemsAsync()
    {
      return await _repository.GetPendingItemsAsync();
    }

    public async Task<IEnumerable<CheckmarkItem>> GetItemsByPriorityAsync(PriorityLevel priority)
    {
      return await _repository.GetItemsByPriorityAsync(priority);
    }
    
  }
}