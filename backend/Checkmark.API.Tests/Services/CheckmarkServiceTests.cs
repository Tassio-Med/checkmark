using Checkmark.API.Models;
using Checkmark.API.Repositories;
using Checkmark.API.Interfaces;
using Checkmark.API.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Checkmark.API.Tests.Services
{
    public class CheckmarkServiceTests
    {
        private readonly Mock<ICheckmarkRepository> _mockRepository;
        private readonly CheckmarkService _service;

        public CheckmarkServiceTests()
        {
            _mockRepository = new Mock<ICheckmarkRepository>();
            
            _service = new CheckmarkService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetItemByIdAsync_Should_Return_Item_When_Exists()
        {
            var expectedItem = new CheckmarkItem 
            { 
                Id = 1, 
                Title = "Test Item", 
                Description = "Test Description" 
            };
            
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                          .ReturnsAsync(expectedItem);

            var result = await _service.GetItemByIdAsync(1);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Test Item");
            
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetItemByIdAsync_Should_Return_Null_When_Not_Exists()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                          .ReturnsAsync((CheckmarkItem?)null);

            var result = await _service.GetItemByIdAsync(999);

            result.Should().BeNull();
            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetAllItemsAsync_Should_Return_All_Items()
        {
            var expectedItems = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "Item 1" },
                new CheckmarkItem { Id = 2, Title = "Item 2" }
            };
            
            _mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(expectedItems);

            var result = await _service.GetAllItemsAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(i => i.Title == "Item 1");
            result.Should().Contain(i => i.Title == "Item 2");
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateItemAsync_Should_Call_AddAsync_And_Return_Item()
        {
            var newItem = new CheckmarkItem 
            { 
                Title = "New Item", 
                Description = "New Description" 
            };

      _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<CheckmarkItem>()));

            var result = await _service.CreateItemAsync(newItem);

            result.Should().Be(newItem); 
            result.Title.Should().Be("New Item");
            
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<CheckmarkItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateItemAsync_Should_Call_UpdateAsync_And_Return_Item()
        {
            var existingItem = new CheckmarkItem 
            { 
                Id = 1, 
                Title = "Original", 
                Description = "Original" 
            };
            
            var updatedItem = new CheckmarkItem 
            { 
                Id = 1, 
                Title = "Updated", 
                Description = "Updated",
                IsCompleted = true
            };
            
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<CheckmarkItem>()))
                          .ReturnsAsync(updatedItem);

            var result = await _service.UpdateItemAsync(updatedItem);

            result.Should().NotBeNull();
            result.Title.Should().Be("Updated");
            result.IsCompleted.Should().BeTrue();
            
            _mockRepository.Verify(repo => repo.UpdateAsync(updatedItem), Times.Once);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Call_DeleteAsync_And_Return_True_When_Successful()
        {
            _mockRepository.Setup(repo => repo.DeleteAsync(1))
                          .ReturnsAsync(true);

            var result = await _service.DeleteItemAsync(1);

            result.Should().BeTrue();
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_False_When_Item_Not_Found()
        {
            _mockRepository.Setup(repo => repo.DeleteAsync(999))
                          .ReturnsAsync(false);

            var result = await _service.DeleteItemAsync(999);

            result.Should().BeFalse();
            _mockRepository.Verify(repo => repo.DeleteAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetCompletedItemsAsync_Should_Return_Only_Completed_Items()
        {
            var completedItems = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "Completed 1", IsCompleted = true },
                new CheckmarkItem { Id = 2, Title = "Completed 2", IsCompleted = true }
            };
            
            _mockRepository.Setup(repo => repo.GetCompletedItemsAsync())
                          .ReturnsAsync(completedItems);

            var result = await _service.GetCompletedItemsAsync();

            result.Should().HaveCount(2);
            result.Should().OnlyContain(i => i.IsCompleted == true);
            _mockRepository.Verify(repo => repo.GetCompletedItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPendingItemsAsync_Should_Return_Only_Pending_Items()
        {
            var pendingItems = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "Pending 1", IsCompleted = false },
                new CheckmarkItem { Id = 2, Title = "Pending 2", IsCompleted = false }
            };
            
            _mockRepository.Setup(repo => repo.GetPendingItemsAsync())
                          .ReturnsAsync(pendingItems);

            var result = await _service.GetPendingItemsAsync();

            result.Should().HaveCount(2);
            result.Should().OnlyContain(i => i.IsCompleted == false);
            _mockRepository.Verify(repo => repo.GetPendingItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetItemsByPriorityAsync_Should_Return_Items_With_Specific_Priority()
        {
            var highPriorityItems = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "High 1", Priority = PriorityLevel.High },
                new CheckmarkItem { Id = 2, Title = "High 2", Priority = PriorityLevel.High }
            };
            
            _mockRepository.Setup(repo => repo.GetItemsByPriorityAsync(PriorityLevel.High))
                          .ReturnsAsync(highPriorityItems);

            var result = await _service.GetItemsByPriorityAsync(PriorityLevel.High);

            result.Should().HaveCount(2);
            result.Should().OnlyContain(i => i.Priority == PriorityLevel.High);
            _mockRepository.Verify(repo => repo.GetItemsByPriorityAsync(PriorityLevel.High), Times.Once);
        }
    }
}