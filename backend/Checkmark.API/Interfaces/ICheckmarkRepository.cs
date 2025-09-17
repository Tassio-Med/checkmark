using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Checkmark.API.Models;

namespace Checkmark.API.Interfaces
{
  public interface ICheckmarkRepository : IRepository<CheckmarkItem>
  {
    Task<IEnumerable<CheckmarkItem>> GetCompletedItemsAsync();
    Task<IEnumerable<CheckmarkItem>> GetPendingItemsAsync();
    Task<IEnumerable<CheckmarkItem>> GetItemsByPriorityAsync(PriorityLevel priority);

  }
}