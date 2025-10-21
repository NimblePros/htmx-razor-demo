using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class GreetingModel : PageModel
{
    private static readonly string[] Messages =
    {
        "Hola", "Hello", "Guten Tag", "Bonjour", "Ciao", "Namaste"
    };

    public IActionResult OnGetMessage()
    {
        var message = Messages[new Random().Next(Messages.Length)];
        return Content($"<p>{message}!</p>", "text/html");
    }

    public IActionResult OnPostSubmit(string name)
    {
        return Content($"<p>Thank you, {name}!</p>", "text/html");
    }
}