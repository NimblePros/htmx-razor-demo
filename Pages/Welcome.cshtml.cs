using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class WelcomeModel : PageModel
{
    private static readonly string[] Messages =
    {
        // English
        "Hello!", "Hi!", "Hey!", "Greetings!",

        // French
        "Bonjour!", "Salut!",

        // German
        "Hallo!", "Guten Tag!",

        // Spanish
        "¡Hola!", "¡Qué tal!",

        // Latin
        "Salve!", "Ave!",

        // Portuguese
        "Olá!", "Oi!",

        // Greek
        "Γειά σου!", "Χαίρε!",

        // Polish
        "Cześć!", "Witaj!",

        // Italian
        "Ciao!", "Salve!"
    }   ;

    public IActionResult OnGetMessage()
    {
        var message = Messages[new Random().Next(Messages.Length)];
        return Content($"<p>{message}</p>", "text/html");
    }
}