using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[IgnoreAntiforgeryToken] // Avoid 400 errors for AJAX
public class ErrorDemoModel : PageModel
{
    public IActionResult OnPost([FromForm] int number)
    {
        // Simulate server validation error
        if (number < 0)
        {
            return BadRequest("Number cannot be negative!");
        }
        if (number > 100)
        {
            return BadRequest("Number cannot exceed 100!");
        }

        // Success: return a friendly message
        return Content($"<div class='alert alert-success'>You entered: {number}</div>", "text/html");
    }
}
