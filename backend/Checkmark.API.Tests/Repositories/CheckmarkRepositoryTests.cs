using Checkmark.API.Data;
using Checkmark.API.Models;
using Checkmark.API.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;
using System.Runtime.CompilerServices;

namespace Checkmark.API.Tests.Repositories
{
  public class CheckmarkRepositoryTests : IDisposable
  {
    private readonly DataDbContext _context;
    private readonly CheckmarkRepository _repository;

    public CheckmarkRepositoryTests()
    {
      var options = new DbContextOptionsBuilder<DataDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

      _context = new DataDbContext(options);
      _repository = new CheckmarkRepository(_context);
      _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
      _context.Database.EnsureDeleted();
      _context.Dispose();
    }

    [Fact]
    public async Task AddAsync_Should_Add_Item_To_Database()
    {
      var item = new CheckmarkItem
      {
        Title = "Test Item",
        Description = "Test Description",
        IsCompleted = false,
        Priority = PriorityLevel.Medium
      };

      await _repository.AddAsync(item);

      var itemsDb = await _context.CheckmarkItems.ToListAsync();
      itemsDb.Should().HaveCount(1);
      itemsDb[0].Title.Should().Be("Test Item");
      itemsDb[0].Description.Should().Be("Test Description");
      itemsDb[0].Description.Should().Be("Test Description");
      itemsDb[0].Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Item_When_Exists()
    {
      var item = new CheckmarkItem
      {
        Title = "Test Item",
        Description = "Test Description",
      };

      await _repository.AddAsync(item);
      var itemId = item.Id;

      var result = await _repository.GetByIdAsync(itemId);

      result.Should().NotBeNull();
      result.Id.Should().Be(itemId);
      result.Title.Should().Be("Test Item");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Item_Does_Not_Exist()
    {
      var result = await _repository.GetByIdAsync(999);

      result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Items()
    {
      var item1 = new CheckmarkItem { Title = "Item 1", Description = "Desc 1" };
      var item2 = new CheckmarkItem { Title = "Item 2", Description = "Desc 2" };

      await _repository.AddAsync(item1);
      await _repository.AddAsync(item2);

      var results = await _repository.GetAllAsync();

      results.Should().HaveCount(2);
      results.Should().Contain(i => i.Title == "Item 1");
      results.Should().Contain(i => i.Title == "Item 2");
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_Item()
    {
      var item = new CheckmarkItem
      {
        Title = "Original",
        Description = "Original",
      };

      await _repository.AddAsync(item);
      
      item.Title = "Updated";
      item.Description = "Updated";
      item.IsCompleted = true;

      await _repository.UpdateAsync(item);

      var updatedItem = await _repository.GetByIdAsync(item.Id);

      updatedItem.Should().NotBeNull("because the item should exist after update");
      updatedItem!.Title.Should().Be("Updated");
      updatedItem!.Description.Should().Be("Updated");
      updatedItem!.IsCompleted.Should().BeTrue();
      updatedItem!.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Item_From_Database()
    {
      var item = new CheckmarkItem
      {
        Title = "To Be Deleted",
        Description = "To Be Deleted",
      };

      await _repository.AddAsync(item);
      var itemId = item.Id;

      await _repository.DeleteAsync(itemId);

      var deleted = await _repository.GetByIdAsync(itemId);
      deleted.Should().BeNull();

      var itemsDb = await _repository.GetAllAsync();
      itemsDb.Should().BeEmpty();
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_True_For_Existing_Item()
    {
      var item = new CheckmarkItem
      {
        Title = "Exists",
        Description = "Exists",
      };

      await _repository.AddAsync(item);
      var itemId = item.Id;

      var exists = await _repository.ExistsAsync(itemId);

      exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_False_For_NonExisting_Item()
    {
      var exists = await _repository.ExistsAsync(999);

      exists.Should().BeFalse();
    }
  }
}