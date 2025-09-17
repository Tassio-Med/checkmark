/* using System.Collections.Generic;
using System.Threading.Tasks;
using Checkmark.API.Models;

namespace Checkmark.API.Interfaces
{
  public interface IRepository<T> where T : BaseEntity
  {
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
  }
} */

using System.Collections.Generic;
using System.Threading.Tasks;
using Checkmark.API.Models;

namespace Checkmark.API.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}