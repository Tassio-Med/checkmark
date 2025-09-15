using System.Diagnostics;

namespace Checkmark.API.Models
{
  public class CheckmarkItem : BaseEntity
  {
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    public override string GetEntityType()
    {
      return "Item";
    }

    public enum PriorityLevel
    {
      Low,
      Medium,
      High
    }
  }

  
}