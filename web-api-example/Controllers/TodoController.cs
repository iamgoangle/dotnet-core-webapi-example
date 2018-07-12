using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using web_api_example.Models;

namespace web_api_example.Controllers {
  [Route ("api/[controller]")]
  [ApiController]

  public class TodoController : ControllerBase {
    private readonly TodoContext _context;

    // Constructor
    public TodoController (TodoContext context) {
      _context = context;

      if (_context.TodoItems.Count () == 0) {
        _context.TodoItems.Add (new TodoItem { Name = "Item 1" });
        _context.SaveChanges ();
      }
    }

    // GET All todo from in-memory database
    [HttpGet]
    public ActionResult<List<TodoItem>> GetAll () {
      return _context.TodoItems.ToList ();
    }

    // GET todo by id
    [HttpGet ("{id}", Name = "GetTodo")]
    public ActionResult<TodoItem> GetById (long id) {
      var item = _context.TodoItems.Find (id);
      if (item == null) {
        return NotFound ();
      }
      return item;
    }
  }
}