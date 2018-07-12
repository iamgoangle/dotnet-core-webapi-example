using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using web_api_example.Models;

namespace web_api_example.Controllers {
  [Route ("api/[controller]")]
  [ApiController]

  public class TodoController : ControllerBase {
    private readonly TodoContext _context;
    private readonly ILogger _logger;

    // Constructor
    public TodoController (TodoContext context, ILogger<TodoController> logger) {
      _context = context;
      _logger = logger;

      if (_context.TodoItems.Count () == 0) {
        _context.TodoItems.Add (new TodoItem { Name = "Item 1" });
        _context.SaveChanges ();
      }
    }

    // GET All todo from in-memory database
    [HttpGet]
    public ActionResult<List<TodoItem>> GetAll () {
      _logger.LogDebug ("Getting all todo");
      return _context.TodoItems.ToList ();
    }

    // GET todo by id
    [HttpGet ("{id}", Name = "GetTodo")]
    public ActionResult<TodoItem> GetById (long id) {
      _logger.LogInformation ("Getting item {ID}", id);

      var item = _context.TodoItems.Find (id);
      if (item == null) {
        return NotFound ();
      }
      return item;
    }

    // POST
    [HttpPost]
    public IActionResult Create (TodoItem item) {
      _context.TodoItems.Add (item);
      _context.SaveChanges ();

      return CreatedAtRoute ("GetTodo", new TodoItem { Id = item.Id }, item);
    }

    // Update by id
    [HttpPut ("{id}")]
    public IActionResult Update (long id, TodoItem item) {
      var todo = _context.TodoItems.Find (id);
      if (todo == null) {
        return NotFound ();
      }

      todo.IsComplete = item.IsComplete;
      todo.Name = item.Name;

      _context.TodoItems.Update (todo);
      _context.SaveChanges ();

      return Ok (todo);
    }

    // DELETE by id
    [HttpDelete ("{id}")]
    public IActionResult Delete (long id) {
      var todo = _context.TodoItems.Find (id);
      if (todo == null) {
        return NotFound ();
      }

      _context.TodoItems.Remove (todo);
      _context.SaveChanges ();
      return NoContent ();
    }
  }
}