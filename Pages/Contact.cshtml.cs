using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[ValidateAntiForgeryToken]
public class ContactModel : PageModel
{
  public IActionResult OnPostSubmitContact(string name, string email, string message)
  {
    var responseMessage = $"<p>Thanks for reaching out, <a href='mailto:{email}'>{name}</a>! We will get back to you soon.</p>";
    return Content(responseMessage, "text/html");
  }
}