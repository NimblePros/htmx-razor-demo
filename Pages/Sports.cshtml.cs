using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[IgnoreAntiforgeryToken]
public class SportsModel : PageModel
{
  public void OnGet() { }

  public IActionResult OnPost()
  {
    string? sport = Request.Headers["HX-Prompt"].ToString();

    if (string.IsNullOrWhiteSpace(sport))
      sport = "No sport 😅";

    // Return div with same ID for HTMX swap
    return Content($@"<div id=""result"" class=""feedback"">
            Awesome! I love {sport} too! 🏀🏈⚽
        </div>", "text/html");
  }
}
