using IdnetityWebApp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace IdnetityWebApp.Pages.Account
{
    public class loginModel : PageModel
    {
        [BindProperty]
        public Credential? credential { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(credential.UserName=="admin" && credential.Password=="password")
            {
                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department","HR"),
                    new Claim("Admin","true"),
                    new Claim("HRManager","true"),
                    new Claim("EmploymentDate","2022-9-16")

                };
                var authProperties = new AuthenticationProperties 
                {
                    IsPersistent = credential.RememberMe,
                };

                var Identity = new ClaimsIdentity(Claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal=new ClaimsPrincipal(Identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                return RedirectToPage("/Index");

            }
            return Page(); 
        }
      
    }
}
