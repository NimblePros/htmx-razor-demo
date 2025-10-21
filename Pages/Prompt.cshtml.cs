using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
[IgnoreAntiforgeryToken]
public class PromptModel : PageModel
{
  public void OnGet() { }

    // Handle the POST request from HTMX
    public IActionResult OnPostFavoriteDessert()
  {
        string? dessert = Request.Headers["HX-Prompt"].ToString();
        if (string.IsNullOrWhiteSpace(dessert))
            return Content("You didn't tell me your favorite dessert! 🍽️");

        return Content($"Yum! I love {dessert} too! 🍰");
    }
}