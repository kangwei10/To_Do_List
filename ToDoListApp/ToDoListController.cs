namespace ToDoListApp.Controllers;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

public class ToDoRepository
{
    public List<ToDoList> Items { get; } = new();
}


[ApiController]
[Route("[controller]")]
public class ToDoListController : ControllerBase

{
    private readonly ToDoRepository _repo;

    public ToDoListController(ToDoRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [EndpointDescription("Retrieves the full list of to-do items.")]
    public ActionResult<List<ToDoList>> Get()
    {
        return _repo.Items;
    }

    [HttpPost]
    [EndpointDescription("Add a new to-do item to the to-do list.")]
    public ActionResult<List<ToDoList>> Post([FromBody] ToDoList item)
    {   
        if (string.IsNullOrWhiteSpace(item.Content))
        {
            return BadRequest("To-do list content cannot be empty");
        }

        _repo.Items.Add(item);
        return _repo.Items;
    }

    [HttpPut("{id}")]
    [EndpointDescription("Update a to-do item from the to-do list using id.")]
    public ActionResult<List<ToDoList>> Put(Guid id, [FromBody] ToDoList item)
    { 
        var index = _repo.Items.FindIndex(t => t.Id == id);
            
        if (index == -1)
            return NotFound($"Item with ID {id} not found");

        item.Id = id;
        _repo.Items[index] = item;
        return _repo.Items;
    }

    [HttpDelete("{id}")]
    [EndpointDescription("Delete a to-do item from the to-do list using id.")]
    public ActionResult<List<ToDoList>> Delete(Guid id)
    { 
        var index = _repo.Items.FindIndex(t => t.Id == id);
            
        if (index == -1)
            return NotFound($"Item with ID {id} not found");

        _repo.Items.RemoveAt(index);
        return _repo.Items;
    }
}