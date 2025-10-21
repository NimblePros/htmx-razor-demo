using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[IgnoreAntiforgeryToken] // Class-level
public class TodoModel : PageModel
{
    public static List<TodoItem> TasksStore { get; } = new();

    public List<TodoItem> Tasks => TasksStore;

    // POST - Add new task
    public IActionResult OnPost([FromForm] string title, [FromForm] string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest();

        var task = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description
        };
        TasksStore.Add(task);

        return TaskHtml(task);
    }

    // PUT - Full replace (Title + Description)
    public IActionResult OnPut(Guid id)
    {
        var task = TasksStore.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        string? prompt = Request.Headers["HX-Prompt"];
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            var parts = prompt.Split('|', 2);
            if (parts.Length == 2)
            {
                task.Title = parts[0].Trim();
                task.Description = parts[1].Trim();
            }
        }

        return TaskHtml(task);
    }

    // PATCH - Description only
    public IActionResult OnPatch(Guid id)
    {
        var task = TasksStore.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        string? newDesc = Request.Headers["HX-Prompt"];
        if (!string.IsNullOrWhiteSpace(newDesc))
            task.Description = newDesc;

        return TaskHtml(task);
    }

    // DELETE - Remove task
    public IActionResult OnDelete(Guid id)
    {
        var task = TasksStore.FirstOrDefault(t => t.Id == id);
        if (task != null) TasksStore.Remove(task);

        return new EmptyResult();
    }

    private ContentResult TaskHtml(TodoItem task)
    {
        return Content($@"
<li class=""list-group-item task-item htmx-added"" id=""task-{task.Id}"">
    <strong>{task.Title}</strong> - {task.Description}
    <div class=""mt-1"">
        <button hx-put=""/Todo/{task.Id}"" hx-target=""#task-{task.Id}"" hx-swap=""outerHTML"" hx-prompt=""Title | Description"" class=""btn btn-sm btn-warning"">Edit (PUT)</button>
        <span class=""spinner"" hx-indicator>⏳</span>

        <button hx-patch=""/Todo/{task.Id}"" hx-target=""#task-{task.Id}"" hx-swap=""outerHTML"" hx-prompt=""New description"" class=""btn btn-sm btn-info"">Edit Desc (PATCH)</button>
        <span class=""spinner"" hx-indicator>⏳</span>

        <button hx-delete=""/Todo/{task.Id}"" hx-target=""#task-{task.Id}"" hx-swap=""outerHTML"" class=""btn btn-sm btn-danger"">Delete</button>
        <span class=""spinner"" hx-indicator>⏳</span>
    </div>
</li>", "text/html");
    }

    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
