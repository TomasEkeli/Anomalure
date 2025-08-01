using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anomalure.Web.Pages.Ping;

public class PingModel : PageModel
{
  public IActionResult OnGet()
  {
    if (Request.Headers["HX-Request"].Count > 0)
    {
      return Content(
          content: $"<strong>Pong</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}<br/>",
          contentType: "text/html"
      );
    }

    return Page();
  }
}
