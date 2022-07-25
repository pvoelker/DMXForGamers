using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DMXForGamers.Web.Pages
{
    public class IndexModel : PageModel
    {
        public Models.Main Data = null;

        public void OnGet()
        {
            Data = Models.Main.Instance;
        }
    }
}
