using System.Collections.Generic;
using System.Threading.Tasks;
using Checkmark.API.Models;


namespace Checkmark.API.Services
{
  public interface ICheckmarkService
  {
    Task<CheckmarkItem?> GetItemByIdAsync(int id);
    Task<IEnumerable<CheckmarkItem>> GetAllItemsAsync();
    Task<CheckmarkItem> CreateItemAsync(CheckmarkItem item);
    Task<CheckmarkItem?> UpdateItemAsync(CheckmarkItem item);
    Task<bool> DeleteItemAsync(int id);
    Task<IEnumerable<CheckmarkItem>> GetCompletedItemsAsync();
    Task<IEnumerable<CheckmarkItem>> GetPendingItemsAsync();
    Task<IEnumerable<CheckmarkItem>> GetItemsByPriorityAsync(PriorityLevel priority);
  }
}