using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[IgnoreAntiforgeryToken]
public class TodoOOBModel : PageModel
{
    public static List<TodoItem> TasksStore { get; } = new();

    public List<TodoItem> Tasks => TasksStore;

    // POST - Add
    public IActionResult OnPost([FromForm] string title, [FromForm] string description)
    {
        var task = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description
        };
        TasksStore.Add(task);

        return TaskHtml(task);
    }

    // PUT - Full
    public IActionResult OnPut(Guid id)
    {
        var task = TasksStore.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        var prompt = Request.Headers["HX-Prompt"];
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            var parts = prompt.ToString().Split('|', 2);
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

        var newDesc = Request.Headers["HX-Prompt"];
        if (!string.IsNullOrWhiteSpace(newDesc))
            task.Description = newDesc!;

        return TaskHtml(task);
    }

    // DELETE - remove task and update count / lastModified at top
    public IActionResult OnDelete(Guid id)
    {
        var task = TasksStore.FirstOrDefault(t => t.Id == id);
        if (task != null) TasksStore.Remove(task);

        // Update task count (OOB)
        var taskCountOOB = $@"<span id=""taskCount"" hx-swap-oob=""true"">{TasksStore.Count}</span>";

        // Update lastModified at top
        var lastModifiedOOB = $@"<span id=""lastModified"" hx-swap-oob=""true"">Task deleted @ {DateTime.Now:T}</span>";

        return Content(taskCountOOB + lastModifiedOOB, "text/html");
    }

    private ContentResult TaskHtml(TodoItem task)
    {
        var taskCountOOB = $@"<span id=""taskCount"" hx-swap-oob=""true"">{TasksStore.Count}</span>";
        var lastModifiedOOB = $@"<span id=""lastModified"" hx-swap-oob=""true"">{task.Title} @ {DateTime.Now:T}</span>";

        return Content($@"
<li class=""list-group-item task-item htmx-added"" id=""task-{task.Id}"">
    <strong>{task.Title}</strong> - {task.Description}
    <div class=""mt-1"">
        <button hx-put=""/TodoOOB/{task.Id}"" hx-target=""#task-{task.Id}"" hx-swap=""outerHTML"" hx-prompt=""Title | Description"" class=""btn btn-sm btn-warning"">Edit (PUT)</button>
        <span class=""spinner"" hx-indicator>⏳</span>

        <button hx-patch=""/TodoOOB/{task.Id}"" hx-target=""#task-{task.Id}"" hx-swap=""outerHTML"" hx-prompt=""New description"" class=""btn btn-sm btn-info"">Edit Desc (PATCH)</button>
        <span class=""spinner"" hx-indicator>⏳</span>

        <button hx-delete=""/TodoOOB/{task.Id}"" hx-target=""#task-{task.Id}"" hx-swap=""outerHTML"" class=""btn btn-sm btn-danger"">Delete</button>
        <span class=""spinner"" hx-indicator>⏳</span>
    </div>
</li>

{taskCountOOB}
{lastModifiedOOB}", "text/html");
    }

    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
