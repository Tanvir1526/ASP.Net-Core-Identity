using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdnetityWebApp.Pages
{
    [Authorize(policy: "HRDerpartment")]
    public class HumanResourceModel : PageModel
    {
       
        public void OnGet()
        {
        }
    }
}
