using Checkmark.API.Models;
using FluentAssertions;
using Xunit;

namespace Checkmark.API.Tests.Models
{
    public class CheckmarkItemTests
    {
        [Fact]
        public void CheckmarkItem_Should_Have_Correct_Default_Values()
        {
            var item = new CheckmarkItem();
            
            item.Title.Should().BeEmpty();
            item.Description.Should().BeEmpty();
            item.IsCompleted.Should().BeFalse();
            item.Priority.Should().Be(PriorityLevel.Medium);
            item.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            item.UpdatedAt.Should().BeNull();
        }
        
        [Fact]
        public void CheckmarkItem_GetEntityType_Should_Return_Item()
        {
            var item = new CheckmarkItem();
            
            var result = item.GetEntityType();
            
            result.Should().Be("Item");
        }
        
        [Theory]
        [InlineData("Tarefa importante", "Descrição detalhada", PriorityLevel.High)]
        [InlineData("Tarefa normal", "Descrição simples", PriorityLevel.Medium)]
        [InlineData("Tarefa simples", "", PriorityLevel.Low)]
        public void CheckmarkItem_Should_Accept_Valid_Values(
            string title, string description, PriorityLevel priority)
        {
            var item = new CheckmarkItem
            {
                Title = title,
                Description = description,
                Priority = priority,
                IsCompleted = true
            };
            
            item.Title.Should().Be(title);
            item.Description.Should().Be(description);
            item.Priority.Should().Be(priority);
            item.IsCompleted.Should().BeTrue();
        }
        
        [Fact]
        public void CheckmarkItem_Should_Handle_DueDate_Correctly()
        {
            var dueDate = DateTime.UtcNow.AddDays(7);
            
            var item = new CheckmarkItem
            {
                DueDate = dueDate
            };
            
            item.DueDate.Should().Be(dueDate);
            item.DueDate?.Date.Should().Be(dueDate.Date);
        }
    }
}