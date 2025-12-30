using Xunit;
using ToDoListApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace ToDoListApp.Tests;

public class ToDoListControllerTests
{
    private readonly ToDoListController _controller = new ToDoListController(new ToDoRepository());
    [Fact]
    public void Get_Init_List()
    {
        var result = _controller.Get();

        var list = Assert.IsType<List<ToDoList>>(result.Value);
        Assert.Empty(list);
    }

    [Fact]
    public void Add_Single_Item()
    {
        var item = new ToDoList
        {
            Content = "Buy milk",
            DueDate = DateOnly.FromDateTime(DateTime.Today),
            Priority = Priority.Medium,
            Status = TaskStatus.Pending
        };

        var result = _controller.Post(item);
        var list = Assert.IsType<List<ToDoList>>(result.Value);

        Assert.Single(list);
        Assert.Equal(item.Content, list[0].Content);
        Assert.Equal(item.DueDate, list[0].DueDate);
        Assert.Equal(item.Priority, list[0].Priority);
        Assert.Equal(item.Status, list[0].Status);
    }

    [Fact]
    public void Add_Multiple_Item()
    {
        var item1 = new ToDoList { 
            Content = "Task 1", 
            DueDate = DateOnly.FromDateTime(DateTime.Today), 
            Priority = Priority.Low, 
            Status = TaskStatus.Pending 
        };
        var item2 = new ToDoList { 
            Content = "Task 2", 
            DueDate = DateOnly.FromDateTime(DateTime.Today), 
            Priority = Priority.High, 
            Status = TaskStatus.InProgress 
        };

        _controller.Post(item1);
        var result = _controller.Post(item2);

        var list = Assert.IsType<List<ToDoList>>(result.Value);
        Assert.Equal(2, list.Count);
        Assert.Equal(item1.Content, list[0].Content);
        Assert.Equal(item1.DueDate, list[0].DueDate);
        Assert.Equal(item1.Priority, list[0].Priority);
        Assert.Equal(item1.Status, list[0].Status);

        Assert.Equal(item2.Content, list[1].Content);
        Assert.Equal(item2.DueDate, list[1].DueDate);
        Assert.Equal(item2.Priority, list[1].Priority);
        Assert.Equal(item2.Status, list[1].Status);
    }

    [Fact]
    public void Returns_BadRequest_When_Content_Is_Empty()
    {
        var item = new ToDoList
        {
            Content = "  ",
            DueDate = DateOnly.FromDateTime(DateTime.Today),
            Priority = Priority.High,
            Status = TaskStatus.Pending
        };

        var result = _controller.Post(item);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("To-do list content cannot be empty", badRequest.Value);
    }

    [Fact]
    public void Updates_Item_When_Id_Exists()
    {
        var original = new ToDoList { Content = "Original Task", DueDate = DateOnly.FromDateTime(DateTime.Today), Priority = Priority.Medium, Status = TaskStatus.Pending };
        _controller.Post(original);

        var updated = new ToDoList { Content = "Updated Task", DueDate = original.DueDate, Priority = Priority.High, Status = TaskStatus.InProgress };

        var result = _controller.Put(original.Id, updated);
        var list = Assert.IsType<List<ToDoList>>(result.Value);

        Assert.Single(list);
        Assert.Equal(updated.Content, list[0].Content);
        Assert.Equal(updated.DueDate, list[0].DueDate);
        Assert.Equal(updated.Priority, list[0].Priority);
        Assert.Equal(updated.Status, list[0].Status);
        Assert.Equal(original.Id, list[0].Id);
    }

    [Fact]
    public void Put_Returns_NotFound_When_Id_Does_Not_Exist()
    {
        var fakeId = Guid.NewGuid();

        var result = _controller.Put(fakeId, new ToDoList {
            Content = "Test",
            Priority = Priority.High,
            Status = TaskStatus.InProgress
        });

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public void Removes_Item_When_Id_Exists()
    {
        var item = new ToDoList { Content = "Delete Task", DueDate = DateOnly.FromDateTime(DateTime.Today), Priority = Priority.Low, Status = TaskStatus.Pending };
        var postResult = _controller.Post(item);
        var list = Assert.IsType<List<ToDoList>>(postResult.Value);
        Assert.Single(list);  // initially got one

        var deleteResult = _controller.Delete(item.Id);
        list = Assert.IsType<List<ToDoList>>(deleteResult.Value);
        Assert.Empty(list); // empty after deleted
    }

    [Fact]
    public void Delete_Returns_NotFound_When_Id_Does_Not_Exist()
    {
        var fakeId = Guid.NewGuid();

        var result = _controller.Delete(fakeId);
        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

        Assert.Contains("not found", notFound.Value.ToString());
    }
}
