using Checkmark.API.Controllers;
using Checkmark.API.Models;
using Checkmark.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Checkmark.API.Tests.Controllers
{
    public class CheckmarkControllerTests
    {
        private readonly Mock<ICheckmarkService> _mockService;
        private readonly CheckmarkController _controller;

        public CheckmarkControllerTests()
        {
            _mockService = new Mock<ICheckmarkService>();
            _controller = new CheckmarkController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_Items_When_Items_Exist()
        {
            // Arrange
            var items = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "Item 1" },
                new CheckmarkItem { Id = 2, Title = "Item 2" }
            };
            
            _mockService.Setup(service => service.GetAllItemsAsync())
                       .ReturnsAsync(items);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = result.Result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult!.StatusCode.Should().Be(200);
            
            var returnedItems = actionResult.Value as IEnumerable<CheckmarkItem>;
            returnedItems.Should().HaveCount(2);
            _mockService.Verify(service => service.GetAllItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_Should_Return_Ok_With_Item_When_Exists()
        {
            // Arrange
            var item = new CheckmarkItem { Id = 1, Title = "Test Item" };
            
            _mockService.Setup(service => service.GetItemByIdAsync(1))
                       .ReturnsAsync(item);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var actionResult = result.Result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult!.StatusCode.Should().Be(200);
            
            var returnedItem = actionResult.Value as CheckmarkItem;
            returnedItem.Should().NotBeNull();
            returnedItem!.Id.Should().Be(1);
            returnedItem!.Title.Should().Be("Test Item");
            _mockService.Verify(service => service.GetItemByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Item_Not_Exists()
        {
            // Arrange
            _mockService.Setup(service => service.GetItemByIdAsync(999))
                       .ReturnsAsync((CheckmarkItem?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            var actionResult = result.Result as NotFoundResult;
            actionResult.Should().NotBeNull();
            actionResult!.StatusCode.Should().Be(404);
            _mockService.Verify(service => service.GetItemByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task Create_Should_Return_Created_With_Item_When_Successful()
        {
            // Arrange
            var newItem = new CheckmarkItem { Title = "New Item", Description = "New Desc" };
            var createdItem = new CheckmarkItem { Id = 1, Title = "New Item", Description = "New Desc" };
            
            _mockService.Setup(service => service.CreateItemAsync(newItem))
                       .ReturnsAsync(createdItem);

            // Act
            var result = await _controller.Create(newItem);

            // Assert
            var actionResult = result.Result as CreatedAtActionResult;
            actionResult.Should().NotBeNull();
            actionResult!.StatusCode.Should().Be(201);
            actionResult!.ActionName.Should().Be(nameof(CheckmarkController.GetById));
            actionResult!.RouteValues!["id"].Should().Be(1);
            
            var returnedItem = actionResult.Value as CheckmarkItem;
            returnedItem.Should().NotBeNull();
            returnedItem!.Id.Should().Be(1);
            returnedItem!.Title.Should().Be("New Item");
            _mockService.Verify(service => service.CreateItemAsync(newItem), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Return_NoContent_When_Successful()
        {
            // Arrange
            var item = new CheckmarkItem { Id = 1, Title = "Updated", Description = "Updated" };
            
            _mockService.Setup(service => service.UpdateItemAsync(item))
                       .ReturnsAsync(item);

            // Act
            var result = await _controller.Update(1, item);

            // Assert
            // Para ActionResult<T>, precisamos verificar o Value para NoContent
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
            _mockService.Verify(service => service.UpdateItemAsync(item), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Return_BadRequest_When_Id_Mismatch()
        {
            // Arrange
            var item = new CheckmarkItem { Id = 1, Title = "Updated" };

            // Act
            var result = await _controller.Update(999, item);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
            _mockService.Verify(service => service.UpdateItemAsync(It.IsAny<CheckmarkItem>()), Times.Never);
        }

        [Fact]
        public async Task Update_Should_Return_NotFound_When_Item_Not_Exists()
        {
            // Arrange
            var item = new CheckmarkItem { Id = 999, Title = "Non-existent" };
            
            _mockService.Setup(service => service.UpdateItemAsync(item))
                       .ReturnsAsync((CheckmarkItem?)null);

            // Act
            var result = await _controller.Update(999, item);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
            _mockService.Verify(service => service.UpdateItemAsync(item), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Return_NoContent_When_Successful()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteItemAsync(1))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
            _mockService.Verify(service => service.DeleteItemAsync(1), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Item_Not_Exists()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteItemAsync(999))
                       .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
            _mockService.Verify(service => service.DeleteItemAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetCompletedItems_Should_Return_Ok_With_Completed_Items()
        {
            // Arrange
            var completedItems = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "Completed 1", IsCompleted = true },
                new CheckmarkItem { Id = 2, Title = "Completed 2", IsCompleted = true }
            };
            
            _mockService.Setup(service => service.GetCompletedItemsAsync())
                       .ReturnsAsync(completedItems);

            // Act
            var result = await _controller.GetCompletedItems();

            // Assert
            var actionResult = result.Result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult!.StatusCode.Should().Be(200);
            
            var returnedItems = actionResult.Value as IEnumerable<CheckmarkItem>;
            returnedItems.Should().HaveCount(2);
            returnedItems.Should().OnlyContain(i => i.IsCompleted == true);
            _mockService.Verify(service => service.GetCompletedItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByPriority_Should_Return_Ok_With_Priority_Items()
        {
            // Arrange
            var highPriorityItems = new List<CheckmarkItem>
            {
                new CheckmarkItem { Id = 1, Title = "High 1", Priority = PriorityLevel.High },
                new CheckmarkItem { Id = 2, Title = "High 2", Priority = PriorityLevel.High }
            };
            
            _mockService.Setup(service => service.GetItemsByPriorityAsync(PriorityLevel.High))
                       .ReturnsAsync(highPriorityItems);

            // Act
            var result = await _controller.GetByPriority(PriorityLevel.High);

            // Assert
            var actionResult = result.Result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult!.StatusCode.Should().Be(200);
            
            var returnedItems = actionResult.Value as IEnumerable<CheckmarkItem>;
            returnedItems.Should().HaveCount(2);
            returnedItems.Should().OnlyContain(i => i.Priority == PriorityLevel.High);
            _mockService.Verify(service => service.GetItemsByPriorityAsync(PriorityLevel.High), Times.Once);
        }
    }
}