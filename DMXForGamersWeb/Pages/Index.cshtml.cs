using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DMXForGamers.Web.Pages
{
    public class IndexModel : PageModel
    {
        public required Models.Main Data;

        public void OnGet()
        {
            Data = Models.Main.Instance;
        }
    }
}
