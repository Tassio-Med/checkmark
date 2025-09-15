using System;

namespace Checkmark.API.Models
{
  public abstract class BaseEntity
  {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual string GetEntityType()
    {
      return "Base Entity";
    }
  }
}